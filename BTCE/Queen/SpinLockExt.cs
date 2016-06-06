using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Queen
{
    class SpinLockExt:IDisposable
    {
        private readonly SpinLock spinLock;
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
