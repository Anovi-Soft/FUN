using System;
using System.Threading;

namespace Queen
{
    class SpinLockExt : IDisposable
    {
        private SpinLock spinLock;
        private bool lockTaken;
        public SpinLockExt(SpinLock spinLock)
        {
            this.spinLock = spinLock;
            spinLock.Enter(ref lockTaken);
        }

        public void Dispose()
        {
            if (lockTaken) spinLock.Exit(false);
        }
    }
}