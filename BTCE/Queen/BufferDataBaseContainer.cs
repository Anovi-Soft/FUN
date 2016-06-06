using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BTCE.Models;

namespace Queen
{
    class BufferDataBaseContainer: IDisposable
    {
        static SpinLock spinlock = new SpinLock();
        private Queue<Ticker> tickers;
        private readonly ConcurrentDictionary<TradePair, Ticker> lastTickers;
        private readonly Action<IEnumerable<Ticker>> actionDump;
        private const int Capacity = 100;
        public BufferDataBaseContainer(Ticker[] pairs, Action<IEnumerable<Ticker>> actionDump)
        {
            this.actionDump = actionDump;
            tickers = new Queue<Ticker>();
            lastTickers = new ConcurrentDictionary<TradePair, Ticker>
                (pairs.ToDictionary(x => x.TradePair, x => x));
        }

        ~BufferDataBaseContainer()
        {
            Dispose();
        }

        public void Add(Ticker current)
        {
            var prev = lastTickers[current.TradePair];
            if (current.Equals(prev)) return;
            using (new SpinLockExt(spinlock))
            {
                tickers.Enqueue(current);
                if (tickers.Count < Capacity) return;
            }
            DumpBuffer();
        }

        public void SubOldValues(Dictionary<TradePair, Ticker> current)
        {
            Queue<Ticker> unFiltered;
            using (new SpinLockExt(spinlock))
            {
                unFiltered = tickers;
                tickers = new Queue<Ticker>();
            }
            var send = unFiltered.Where(x => current[x.TradePair].Updated < x.Updated)
                .Where(x=>!x.Equals(current[x.TradePair]));
            actionDump(send);
        }
        
        public void DumpBuffer()
        {
            Queue<Ticker> buffer;
            using (new SpinLockExt(spinlock))
            {
                if (tickers.Count == 0) return;
                buffer = tickers;
                tickers = new Queue<Ticker>();
            }
            actionDump(buffer);
        }

        public void Dispose() => DumpBuffer();
    }
}
