#region

using System;
using System.Threading;

#endregion

namespace BlCommon
{
    internal class Threads_Local_Storage_Test_Program
    {
        #region Constructors

        public Threads_Local_Storage_Test_Program()
        {
            Console.WriteLine("****** Threads number:{0} ******", m_threads.Length);
            for (int i = 0; i < m_threads.Length; i++)
            {
                Thread t = new Thread(threadStarter);
                t.Name = "Thread_" + i;
                t.Start();
            }
        }

        #endregion

        #region Private Methods

        private static void RunMain(string[] args)
        {
            new Threads_Local_Storage_Test_Program();
            Thread.Sleep(300);
            Console.WriteLine("****** Press Enter to exit ******");
            Console.ReadLine();
        }

        private void threadStarter()
        {
            CounterSlot = 0; //Must be initialized for each thread
            for (int i = 0; i < 10; i++)
            {
                //Simple counter
                int a = m_counterSimple;
                Thread.Sleep(0);
                a++;
                m_counterSimple = a;
                //Locked counter
                lock (m_syncRoot)
                {
                    int b = m_counterLocked;
                    Thread.Sleep(0);
                    b++;
                    m_counterLocked = b;
                }
                //LocalDataStoreSlot counter 
                int c = CounterSlot;
                Thread.Sleep(0);
                c++;
                CounterSlot = c;
                //ThreadStatic attribute counter
                int d = s_counterStatic;
                Thread.Sleep(0);
                d++;
                s_counterStatic = d;

                Console.WriteLine(
                    "{0} simple={1} locked={2} slot={3} static={4}",
                    Thread.CurrentThread.Name,
                    m_counterSimple,
                    m_counterLocked,
                    CounterSlot,
                    s_counterStatic);
            }
        }

        #endregion

        #region Fields

        [ThreadStatic] private static int s_counterStatic;

        private readonly object m_syncRoot = new object();
        private readonly Thread[] m_threads = new Thread[3];
        private int m_counterLocked;
        private int m_counterSimple;

        #endregion

        private int CounterSlot
        {
            get
            {
                LocalDataStoreSlot localDataStoreSlot = Thread.GetNamedDataSlot("CounterSlot");
                return (int) Thread.GetData(localDataStoreSlot);
            }
            set
            {
                LocalDataStoreSlot localDataStoreSlot = Thread.GetNamedDataSlot("CounterSlot");
                Thread.SetData(localDataStoreSlot, value);
            }
        }
    }
}