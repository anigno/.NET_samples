using Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TasksAndAsync
{
    internal class LimitNumberOfTasks
    {
        SemaphoreSlim sem = new SemaphoreSlim(3);
        Random random = new Random();
        public LimitNumberOfTasks()
        {
            for (int i = 0; i < 5; i++)
            {
                Thread thread = new Thread(ThreadStartMrthod);
                thread.IsBackground = true;
                thread.Start($"thread worker_{i}");
            }

            for (int i = 5; i < 10; i++)
            {
                int internal_i = i;
                Task.Factory.StartNew(() => ThreadStartMrthod($"task worker_{internal_i}"));
            }
        }

        void ThreadStartMrthod(object state)
        {
            sem.Wait();
            string workerName = state as string;
            Logger.Log($"*** starting {workerName} free semaphore: {sem.CurrentCount} ");
            for (int i = 0; i < 5; i++)
            {
                Logger.Log($"{workerName} running");
                Thread.Sleep(random.Next(100, 300));
            }
            sem.Release();
        }
    }
}
