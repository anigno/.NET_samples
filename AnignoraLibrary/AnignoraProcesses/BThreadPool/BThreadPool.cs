using System;
using System.Collections.Generic;
using System.Threading;
using AnignoraCommonAndHelpers;
using AnignoraCommonAndHelpers.Tracers;

namespace AnignoraProcesses.BThreadPool
{
    public class BThreadPool : IStartable
    {
		#region (------  Fields  ------)
        private long m_continueFlag;
        private readonly ManualResetEvent m_eventQueueIsEmpty;
        private readonly Queue<ActionltemBase> m_queueActions;
        private readonly string m_threadPoolName;
        private readonly Thread[] m_threads;
        private long m_threadsInAction;
        private readonly ITracer m_tracer;
		#endregion (------  Fields  ------)

		#region (------  Events  ------)
        public event EventHandler<ActionExceptionEventArgs> ActionExceptionOccured = delegate { };

        /// <summary>
        /// Raised when all retries failed 
        /// </summary>
        public event EventHandler<ActionEventArgs> ActionFailedAll = delegate { };

        public event EventHandler<ActionEventArgs> ActionStarted = delegate { };

        public event EventHandler<ActionEventArgs> ActionSucceeded = delegate { };

        public event EventHandler<ActionEventArgs> ActionTimeout = delegate { };

        public event EventHandler<EventArgs> AllQueuedActionsEnded = delegate { };
		#endregion (------  Events  ------)

		#region (------  Constructors  ------)
        public BThreadPool(ITracer p_tracer, int p_numberOfThreads, string p_threadPoolName)
        {
            m_tracer = p_tracer;
            m_threadPoolName = p_threadPoolName;
            m_queueActions = new Queue<ActionltemBase>();
            m_eventQueueIsEmpty = new ManualResetEvent(false);
            m_threads = new Thread[p_numberOfThreads];
        }
		#endregion (------  Constructors  ------)

		#region (------  Properties  ------)
        public long RunningActionsCount
        {
            get { return Interlocked.Read(ref m_threadsInAction); }
        }
		#endregion (------  Properties  ------)

		#region (------  Public Methods  ------)
        public void EnqueueAction(ActionltemBase p_action)
        {
            lock (m_queueActions)
            {
                m_tracer.Trace("Enqueue new action [{0}]", p_action.Descriptor);
                m_queueActions.Enqueue(p_action);
                m_eventQueueIsEmpty.Set();
            }
        }

        public void Start()
        {
            m_tracer.Trace("Starting");
            Interlocked.Exchange(ref m_continueFlag, 1);
            for (int a = 0; a < m_threads.Length; a++)
            {
                m_threads[a] = new Thread(threadStart);
                m_threads[a].Name = m_threadPoolName + a.ToString("00");
                m_threads[a].IsBackground = true;
            }
            foreach (Thread t in m_threads)
            {
                t.Start();
            }
        }

        public void Stop()
        {
            m_tracer.Trace("Stoping");
            Interlocked.Exchange(ref m_continueFlag, 0);
        }
		#endregion (------  Public Methods  ------)

		#region (------  Private Methods  ------)
        private void actionTimeoutEventHandler(object p_object)
        {
            ActionltemBase actionltemBase = (ActionltemBase) p_object;
            Thread.CurrentThread.Name = "ActionTimeoutThread";
            m_tracer.Trace("[{0}] Timeout:{1} ms", actionltemBase.Descriptor,
                           actionltemBase.TimeoutMs);
            actionltemBase.Abort();
            ActionTimeout(this, new ActionEventArgs(actionltemBase));
        }

        private void threadStart()
        {
            ActionltemBase actionltemBase;
            BThreadPoolTimer timer = new BThreadPoolTimer(actionTimeoutEventHandler);
            while (Interlocked.Read(ref m_continueFlag) == 1)
            {
                lock (m_queueActions)
                {
                    int queueCount = m_queueActions.Count;
                    m_tracer.Trace("QueueCount:{0}", queueCount);
                    if (queueCount == 0)
                    {
                        actionltemBase = null;
                    }
                    else
                    {
                        actionltemBase = m_queueActions.Dequeue();
                    }
                }
                if (actionltemBase == null)
                {
                    if (Interlocked.Read(ref m_threadsInAction) == 0)
                    {
                        AllQueuedActionsEnded(this, new EventArgs());
                    }
                    m_eventQueueIsEmpty.Reset();
                    m_eventQueueIsEmpty.WaitOne();
                }
                else
                {
                    Interlocked.Increment(ref m_threadsInAction);
                    bool isSucceeded = false;
                    int triesLeft = actionltemBase.Retries;
                    while (!isSucceeded && triesLeft > 0)
                    {
                        timer.Change(actionTimeoutEventHandler, actionltemBase, actionltemBase.TimeoutMs, int.MaxValue);
                        try
                        {
                            ActionStarted(this, new ActionEventArgs(actionltemBase));
                            m_tracer.Trace("[{0}] Run", actionltemBase.Descriptor);
                            isSucceeded = actionltemBase.Run();
                            m_tracer.Trace(" [ {0} ] isSucceeded:{1}", actionltemBase.Descriptor, isSucceeded);
                            triesLeft--;
                        }
                        catch (Exception ex)
                        {
                            m_tracer.Trace(" [ {0} ] Exception: {1} ", actionltemBase.Descriptor, ex.Message);
                            ActionExceptionOccured(this, new ActionExceptionEventArgs(actionltemBase, ex));
                            triesLeft--;
                        }
                    }
                    if (isSucceeded)
                        ActionSucceeded(this, new ActionEventArgs(actionltemBase));
                    else
                        ActionFailedAll(this, new ActionEventArgs(actionltemBase));
                    Interlocked.Decrement(ref m_threadsInAction);
                }
            }
            timer.Dispose();
        }
		#endregion (------  Private Methods  ------)
    }
}
