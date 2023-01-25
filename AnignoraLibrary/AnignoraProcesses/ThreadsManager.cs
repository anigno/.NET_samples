using System;
using System.Collections.Generic;
using System.Threading;

namespace AnignoraProcesses
{
    public class ThreadsManager
    {
		#region (------  Fields  ------)
        private readonly Queue<Action> m_actionsQueue = new Queue<Action>();
        private long m_continueFlag = 1;
        private readonly int m_maxRunningThreads;
        private long m_runningThreads;
        private readonly ManualResetEvent m_threadFinishedEvent = new ManualResetEvent(false);
        private readonly ManualResetEvent m_threadRequestEvent = new ManualResetEvent(false);
        private Thread m_threadsCreatorThread;
        private readonly List<Thread> m_threadsList = new List<Thread>();
		#endregion (------  Fields  ------)

		#region (------  Constructors  ------)
        public ThreadsManager(int p_maxRunningThreads)
        {
            m_maxRunningThreads = p_maxRunningThreads;
        }
		#endregion (------  Constructors  ------)

		#region (------  Public Methods  ------)
        public void AddAction(Action p_action)
        {
            lock (m_actionsQueue)
            {
                m_actionsQueue.Enqueue(p_action);
            }
            m_threadRequestEvent.Set();
        }

        public void Start()
        {
            Interlocked.Exchange(ref m_continueFlag, 1);
            m_threadsCreatorThread = new Thread(threadsCreatorThreadStart);
            m_threadsCreatorThread.IsBackground = true;
            m_threadFinishedEvent.Reset();
            m_threadRequestEvent.Reset();
            m_threadsCreatorThread.Start();
        }

        public void Stop()
        {

            Interlocked.Exchange(ref m_continueFlag, 0);
            m_threadRequestEvent.Set();
            m_threadFinishedEvent.Set();
        }
		#endregion (------  Public Methods  ------)

		#region (------  Private Methods  ------)
        private void threadsCreatorThreadStart()
        {
            while (Interlocked.Read(ref m_continueFlag) == 1)
            {
                int queuedActions;
                lock (m_actionsQueue)
                {
                    queuedActions = m_actionsQueue.Count;
                }
                //Check if actions queue is empty to wait for new action 
                if (queuedActions == 0) m_threadRequestEvent.WaitOne(); 
                m_threadRequestEvent.Reset() ;
                //If manager was stopped, return main thread function
                if (Interlocked.Read(ref m_continueFlag) == 0) return;
                //Check if needed to wait for free thread
                if (Interlocked.Read(ref m_runningThreads) >= m_maxRunningThreads) m_threadFinishedEvent.WaitOne();
                m_threadFinishedEvent.Reset();
                //If manager was stopped, return main thread function 
                if (Interlocked.Read(ref m_continueFlag) == 0) return;
                Action actionltem; 
                lock (m_actionsQueue)
                {
                    actionltem = m_actionsQueue.Dequeue();
                }
                Interlocked.Increment(ref m_runningThreads);
                Thread thread = new Thread(threadStartAction);
                lock (m_threadsList)
                {
                    m_threadsList.Add(thread);
                }
                thread.IsBackground = true;
                thread.Start(actionltem);
            }
        }

        private void threadStartAction(object p_object)
        {
            Action action = (Action)p_object;
            action();
            Interlocked.Decrement(ref m_runningThreads);
            m_threadFinishedEvent.Set();
            lock (m_threadsList)
            {
                m_threadsList.Remove(Thread.CurrentThread);
            }
        }
		#endregion (------  Private Methods  ------)
    }
}

