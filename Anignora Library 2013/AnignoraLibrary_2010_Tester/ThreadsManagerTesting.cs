using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using AnignoraProcesses;
using NUnit.Framework;

namespace AnignoraLibrary_2010_Tester
{
    [TestFixture]
    public class ThreadsManagerTesting
    {
        private const int MAX_THREADS = 3;
        private readonly ThreadsManager m_threadsManager = new ThreadsManager(MAX_THREADS);
        public const int THREAD_SLEEP = 100;
        private long m_threadsCounter = 0;
        private readonly Random m_random=new Random(DateTime.Now.Millisecond);

        [Test]
        public void TestThreadsManager()
        {
            m_threadsManager.Start();
            int pNActions = 100;
            runTestThreadsManager(pNActions);
            Thread.Sleep(THREAD_SLEEP * 2 * pNActions / MAX_THREADS);
            Console.WriteLine("------ [{0}] Test Ended {1} actions", DateTime.Now.ToString("hh:mm:ss:fff"), pNActions);
        }


        private void runTestThreadsManager(int p_nActions)
        {
            Console.WriteLine("------ [{0}] Test Started {1} actions", DateTime.Now.ToString("hh:mm:ss:fff"), p_nActions);
            for (int a = 0; a < p_nActions; a++)
            {
                Action action = delegate
                {
                    Interlocked.Increment(ref m_threadsCounter);
                    Console.WriteLine("[{0}] Started {1:00} {2}", DateTime.Now.ToString("hh:mm:ss:fff"), Thread.CurrentThread.ManagedThreadId,Interlocked.Read(ref m_threadsCounter));
                    Assert.LessOrEqual(Interlocked.Read(ref m_threadsCounter), MAX_THREADS);
                    Thread.Sleep(m_random.Next(THREAD_SLEEP,THREAD_SLEEP*2));
                    Interlocked.Decrement(ref m_threadsCounter);
                    Console.WriteLine("[{0}] Ended   {1:00} {2}", DateTime.Now.ToString("hh:mm:ss:fff"), Thread.CurrentThread.ManagedThreadId, Interlocked.Read(ref m_threadsCounter));
                };
                m_threadsManager.AddAction(action);
            }

        }
    }
}
