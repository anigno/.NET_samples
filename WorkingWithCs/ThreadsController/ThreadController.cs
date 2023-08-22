using Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadsController
{
    internal class ThreadController
    {
        private readonly Random random = new Random();
        private readonly ManualResetEvent highMre = new ManualResetEvent(true);
        int highCount = 0;

        public void StartTasks()
        {
            ThreadPool.SetMaxThreads(3, 16);
            foreach(int c in Enumerable.Range(0, 5))
            {
                Task.Factory.StartNew(() => LessImportantWork(new object[] { c }));
                Thread.Sleep(1000);
                Task.Factory.StartNew(() => HighImportantWork(new object[] { c }));
            }
        }

        public void StartWithThreadPool()
        {
            ThreadPool.GetMinThreads(out int a, out int b);
            Console.WriteLine($"{a} {b}");
            ThreadPool.SetMaxThreads(3, 16);
            foreach(int c in Enumerable.Range(0, 5))
            {
                ThreadPool.QueueUserWorkItem(LessImportantWork, new object[] { c });
                Thread.Sleep(1000);
                ThreadPool.QueueUserWorkItem(HighImportantWork, new object[] { c });
            }
        }

        private void LessImportantWork(object state)
        {
            highMre.WaitOne();
            object[] values = state as object[];
            Logger.Log($"LessImportantWork {values[0]} start");
            Thread.Sleep(random.Next(300, 2000));
            Logger.Log($"LessImportantWork {values[0]} end");
        }

        private void HighImportantWork(object state)
        {
            highMre.Reset();
            Interlocked.Increment(ref highCount);
            object[] values = state as object[];
            Logger.Log($"HighImportantWork {values[0]} start");
            Thread.Sleep(random.Next(300, 2000));
            Logger.Log($"HighImportantWork {values[0]} end");
            Interlocked.Decrement(ref highCount);
            if(Interlocked.CompareExchange(ref highCount, 0, 0) == 0)
            {
                highMre.Set();
            }
        }
    }
}
