using Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ReadersWritersProblem
{
    internal class ReaderWriter
    {
        private DataResourceProtection protection = new DataResourceProtection();
        readonly Random random = new Random();
        private int resource = 0;
        internal void Start()
        {
            Logger.Log("starting");
            Task.Factory.StartNew(() => WriterTask("writer_1"));
            Task.Factory.StartNew(() => WriterTask("writer_2"));
            Task.Factory.StartNew(() => ReaderTask("reader_1"));
            Task.Factory.StartNew(() => ReaderTask("reader_2"));
        }

        private void ReaderTask(string name)
        {
            for (int a = 0; a < 100; a++)
            {
                protection.ReadLock();
                Logger.Log($"{name} {a} {resource}");
                protection.ReadUnlock();
                Task.Delay(random.Next(10, 1000)).Wait();
            }
        }

        private void WriterTask(string name)
        {
            for (int a = 0; a < 100; a++)
            {
                protection.WriteLock();
                resource += 1;
                Logger.Log($"{name} {resource}");
                protection.WriteUnlock();
                Task.Delay(random.Next(10, 1000)).Wait();
            }
        }
    }
}
