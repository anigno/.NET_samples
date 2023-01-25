using System.Threading;

namespace AnignoraProcesses
{
    /// <summary>
    /// A Thread pool, instanced, with maximum waiting threads
    /// </summary>
    public class AThreadPool
    {
		#region (------  Static Fields  ------)

        private static readonly object SYNC_ROOT = new object();

		#endregion (------  Static Fields  ------)

		#region (------  Fields  ------)

        private readonly ManualResetEvent[] m_mreRunnerArray;
        private readonly ManualResetEvent[] m_mreWorkerArray;
        private readonly Thread[] m_threadsArray;
        private readonly object[] m_workerParams;
        private readonly AThreadPoolWorkerDelegate[] m_workersFunctions;

		#endregion (------  Fields  ------)

		#region (------  Delegates  ------)

        public delegate void AThreadPoolWorkerDelegate(object p_obj);

		#endregion (------  Delegates  ------)

		#region (------  Constructors  ------)

        public AThreadPool(string p_threadQueueDescriptor, int p_maxThreads ,bool p_isBackground=false)
        {
            m_threadsArray = new Thread[p_maxThreads];
            m_mreWorkerArray = new ManualResetEvent[p_maxThreads];
            m_mreRunnerArray = new ManualResetEvent[p_maxThreads];
            m_workersFunctions=new AThreadPoolWorkerDelegate[p_maxThreads];
            m_workerParams=new object[p_maxThreads];

            for (int a = 0; a < p_maxThreads; a++)
            {
                m_threadsArray[a] = new Thread(workerThreadStart);
                m_threadsArray[a].IsBackground = p_isBackground;
                m_threadsArray[a].Name = p_threadQueueDescriptor + a;
                m_mreWorkerArray[a] = new ManualResetEvent(false);
                m_mreRunnerArray[a] = new ManualResetEvent(true);
                m_threadsArray[a].Start(a);
            }
        }

		#endregion (------  Constructors  ------)

		#region (------  Public Methods  ------)

        public void AbortAll()
        {
            for (int a = 0; a < m_threadsArray.Length; a++)
            {
                m_threadsArray[a].Abort();
                m_mreRunnerArray[a].Set();
                m_mreWorkerArray[a].Set();
            }

        }

        /// <summary>
        /// Waits for a free worker thread and activate it
        /// </summary>
        public void StartFreeWorkerThreadBlocked(AThreadPoolWorkerDelegate p_func,object p_paramObject)
        {
            lock(SYNC_ROOT)
            {
                int freeWorkerIndex = WaitHandle.WaitAny(m_mreRunnerArray);
                m_workersFunctions[freeWorkerIndex] = p_func;
                m_workerParams[freeWorkerIndex] = p_paramObject;
                m_mreRunnerArray[freeWorkerIndex].Reset();
                m_mreWorkerArray[freeWorkerIndex].Set();
            }
        }

		#endregion (------  Public Methods  ------)

		#region (------  Private Methods  ------)

        private void workerThreadStart(object p_objectInt)
        {
            int i = (int) p_objectInt;
            while(true)
            {
                m_mreWorkerArray[i].WaitOne();
                m_workersFunctions[i](m_workerParams[i]);
                m_mreWorkerArray[i].Reset();
                m_mreRunnerArray[i].Set();
            }
        }

		#endregion (------  Private Methods  ------)
    }
}
