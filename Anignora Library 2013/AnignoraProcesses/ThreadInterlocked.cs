using System;
using System.Threading;

namespace AnignoraProcesses
{
    /// <summary>
    /// A Thread wrapper that activate a periodic activity within an interlocked flag.
    /// when interlocked flag is set, activity will not rerun and thread ends
    /// </summary>
    public class ThreadInterlocked
    {
		#region (------  Fields  ------)

        private readonly bool m_isBackground;
        private Thread m_thread;
        private long m_threadContinueFlag = THREAD_CONTINUE_FLAG_FALSE;
        private readonly string m_threadName;
        private readonly Action m_threadedAction;
        private readonly int m_threadLoopSleepMs;
        private readonly ManualResetEvent m_waitForFinishEvent=new ManualResetEvent(false);
        private const long THREAD_CONTINUE_FLAG_FALSE = 0;
        private const long THREAD_CONTINUE_FLAG_TRUE = 1;
        
		#endregion (------  Fields  ------)

		#region (------  Constructors  ------)

        public ThreadInterlocked(string p_threadName,Action p_threadedAction,bool p_isBackground,int p_threadLoopSleepMs=1000)
        {
            m_threadName = p_threadName;
            m_threadedAction = p_threadedAction;
            m_isBackground = p_isBackground;
            m_threadLoopSleepMs = p_threadLoopSleepMs;
        }

		#endregion (------  Constructors  ------)

		#region (------  Properties  ------)

        public bool IsContinue
        {
            get { return Interlocked.Read(ref m_threadContinueFlag) == THREAD_CONTINUE_FLAG_TRUE; }
        }

		#endregion (------  Properties  ------)

		#region (------  Public Methods  ------)

        public void Start()
        {
            m_thread = new Thread(threadStart);
            m_thread.Name = m_threadName;
            m_thread.IsBackground = m_isBackground;
            Interlocked.Exchange(ref m_threadContinueFlag, THREAD_CONTINUE_FLAG_TRUE);
            m_waitForFinishEvent.Reset();
            m_thread.Start();
        }

        public void Stop(bool p_waitForFinish=true,int p_waitForFinishMs=1000)
        {
            Interlocked.Exchange(ref m_threadContinueFlag, THREAD_CONTINUE_FLAG_FALSE);
            if (p_waitForFinish) m_waitForFinishEvent.WaitOne(p_waitForFinishMs);
        }

		#endregion (------  Public Methods  ------)

		#region (------  Private Methods  ------)

        private void threadStart()
        {
            while (Interlocked.Read(ref m_threadContinueFlag) == THREAD_CONTINUE_FLAG_TRUE)
            {
                m_threadedAction();
                if (Interlocked.Read(ref m_threadContinueFlag) == THREAD_CONTINUE_FLAG_TRUE)
                {
                    Thread.Sleep(m_threadLoopSleepMs);
                }
            }
            m_waitForFinishEvent.Set();
        }

		#endregion (------  Private Methods  ------)
    }
}
