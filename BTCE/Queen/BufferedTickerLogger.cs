using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BTCE.Models;

namespace Queen
{
    public class BufferedTickerLogger : IDisposable
    {
        private static volatile BufferedTickerLogger instance;
        private static object syncRoot = new object();

        private SpinLock spinlock;
        private int capacity = 100;
        private Queue<Ticker> tickers;
        private ConcurrentDictionary<TradePair, Ticker> topTickers;
        private Func<bool> dumpCondition;
        private Func<IEnumerable<Ticker>,Task> log;
        private WorkStatus status;

        
        public static BufferedTickerLogger Instance(Action<IEnumerable<Ticker>> logAction)
        {
            if (instance != null) return instance;
            lock (syncRoot)
            {
                if (instance == null)
                    instance = new BufferedTickerLogger(logAction);
            }
            return instance;
        }

        private BufferedTickerLogger(Action<IEnumerable<Ticker>> logAction)
        {
            var topDict = TradePair.AllPairs.Select(x => new Ticker
                {
                    TradePair = x,
                    Updated = 0

                }).ToDictionary(x=>x.TradePair, x=>x);
            topTickers = new ConcurrentDictionary<TradePair, Ticker>(topDict);
            Status = WorkStatus.Normal;
            log = x => Task.Run(() => logAction(x));
            spinlock = new SpinLock();
        }

        ~BufferedTickerLogger()
        {
            Dispose();
        }

        public int Capacity
        {
            get { return capacity; }
            set
            {
                capacity = value;
                if (tickers.Count >= Capacity)
                    AsyncTryDumpBuffer();
            }
        }

        public WorkStatus Status
        {
            get { return status; }
            private set
            {
                using (new SpinLockExt(spinlock))
                {
                    if (value == WorkStatus.Normal)
                        dumpCondition = () => tickers.Count >= Capacity;
                    else
                        dumpCondition = () => false;
                }
                status = value;
            }
        }

        public void Add(Ticker current)
        {
            var prev = topTickers[current.TradePair];
            if (current.UpdateMe(prev)) return;
            using (new SpinLockExt(spinlock))
            {
                tickers.Enqueue(current);
            }
            AsyncTryDumpBuffer();
        }

        private void AsyncTryDumpBuffer()
        {
            if (dumpCondition())
                AsyncDumpBuffer();
        }

        private async void AsyncDumpBuffer()
        {
            Queue<Ticker> buffer;
            using (new SpinLockExt(spinlock))
            {
                if (tickers.Count == 0) return;
                buffer = tickers;
                tickers = new Queue<Ticker>();
            }
            await log(buffer);
        }

        public List<Ticker> BeginSendsStateAndGetTopTickers()
        {
            using (new SpinLockExt(spinlock))
            {
                Status = WorkStatus.SendsState;
                return topTickers.Select(x => x.Value).ToList();
            }
        }

        public void SendsStateSucess() => Dispose();

        public void SendStateError() => Status = WorkStatus.Normal;

        public void WaitTickersCount(int count)
        {
            while (tickers.Count < count)
                Thread.Sleep(100);
        }

        public void EndTakeState(Dictionary<TradePair, Ticker> current)
        {
            Thread.Sleep(100);
            Queue<Ticker> unFiltered;
            using (new SpinLockExt(spinlock))
            {
                unFiltered = tickers;
                tickers = new Queue<Ticker>();
            }
            var send = unFiltered.Where(x => current[x.TradePair].UpdateMe(x));
            log(send);
            Status = WorkStatus.Normal;
        }

        public void Dispose()
        {
            if (status == WorkStatus.Dispose) return;
            AsyncDumpBuffer();
            Status = WorkStatus.Dispose;
        }
        
        public enum WorkStatus
        {
            Dispose,
            Normal,
            SendsState,
            TakeState
        };
    }
    
}
