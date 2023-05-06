#region

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

#endregion

namespace TimedQueue
{
    /// <summary>
    /// A Queue of IQueueTimedItem items that has a property DequeueTime, that determine the time for that next item to be dequeued
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class QueueTimedDequeue<T> where T : IQueueTimedItem
    {
        #region Public Methods

        public void Start()
        {
            if (m_cancellationTokenSource == null || m_cancellationTokenSource.IsCancellationRequested)
            {
                m_cancellationTokenSource = new CancellationTokenSource();
                Task.Factory.StartNew(DequeueHandler, m_cancellationTokenSource.Token);
            }
        }

        public void Stop()
        {
            if (m_cancellationTokenSource == null) return;
            m_cancellationTokenSource.Cancel(true);
        }

        public void Enqueue(T p_item)
        {
            lock (m_queue)
            {
                m_queue.Enqueue(p_item);
                m_queueEmptyWaitEvent.Set();
            }
        }

        #endregion

        #region Events

        public event EventHandler<T> OnDequeue = delegate { };

        #endregion

        #region Private Methods

        private async void DequeueHandler()
        {
            while (!m_cancellationTokenSource.IsCancellationRequested)
            {
                m_queueEmptyWaitEvent.WaitOne();
                T dequeueItem;
                lock (m_queue)
                {
                    dequeueItem = m_queue.Dequeue();
                    if (m_queue.Count == 0) m_queueEmptyWaitEvent.Reset();
                }
                DateTime now = DateTime.Now;
                TimeSpan timeSpanDelay = (dequeueItem.DequeueTime - now);
                if (timeSpanDelay < m_zeroMilliseconds) timeSpanDelay = m_zeroMilliseconds;
                try
                {
                    await Task.Delay(timeSpanDelay, m_cancellationTokenSource.Token);
                }
                catch (TaskCanceledException)
                {
                    return;
                }
                OnDequeue(this, dequeueItem);
            }
        }

        #endregion

        #region Fields

        private readonly TimeSpan m_zeroMilliseconds = TimeSpan.FromMilliseconds(0);
        private readonly Queue<T> m_queue = new Queue<T>();
        private readonly ManualResetEvent m_queueEmptyWaitEvent = new ManualResetEvent(false);
        private CancellationTokenSource m_cancellationTokenSource;

        #endregion
    }
}