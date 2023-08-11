using System;
using System.Linq;
using System.Threading;

namespace ReadersWritersProblem
{
    internal class DataResourceProtection
    {
        readonly Semaphore readSemaphore = new Semaphore(1, 1);
        readonly Semaphore writeSemaphore = new Semaphore(1, 1);
        int readers = 0;

        public void ReadLock()
        {
            readSemaphore.WaitOne();
            readers++;
            if (readers == 1)
                writeSemaphore.WaitOne();
            readSemaphore.Release();
        }
        public void ReadUnlock()
        {
            readSemaphore.WaitOne();
            readers--;
            if (readers == 0) writeSemaphore.Release();
            readSemaphore.Release();

        }
        public void WriteLock()
        {
            writeSemaphore.WaitOne();
        }
        public void WriteUnlock()
        {
            writeSemaphore.Release();
        }
    }
}
