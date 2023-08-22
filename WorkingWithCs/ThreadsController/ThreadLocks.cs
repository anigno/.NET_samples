using Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadsController
{
    internal class ThreadLocks
    {
        object lockObject = new object();
        public ThreadLocks()
        {
            for (int i = 0; i < 5; i++)
            {
                Thread thread = new Thread(ThreadStartMethod);
                thread.IsBackground = true;
                thread.Start(i);
                Thread.Sleep(300);
            }
        }

        private void ThreadStartMethod(object state)
        {
            if (Monitor.TryEnter(lockObject, 900))
            {
                Logger.Log($"{state} entered");
                Thread.Sleep(1000);
                Monitor.Exit(lockObject);
                Logger.Log($"{state} exited");
            }
            else
            {
                Logger.Log($"{state} couldn't enter");
            }
        }
    }
}
