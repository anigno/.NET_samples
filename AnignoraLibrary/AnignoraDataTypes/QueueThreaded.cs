using System;
using System.Collections.Generic;
using System.Threading;

namespace AnignoraDataTypes
{
    /// <summary>
    /// Periodic enqueuing
    /// </summary>
    [Obsolete]
    public class QueueThreaded<T> : List<T> where T : IComparable
    {
        #region Constructors

        public QueueThreaded(int periodicIntervalMs)
            : this()
        {
            _periodicInterval = periodicIntervalMs;
        }

        public QueueThreaded()
        {
            _periodicInterval = 1000;
            _timerThread = new Thread(TimerThreadStart);
            _timerThread.Name = "QueueThreaded " + base.GetHashCode();
            _continuTimerLoop = true;
            _enablePeriodicEvent = true;
            _timerThread.Start();
        }

        #endregion

        #region Public Methods

        public void AbortPeriodicThread()
        {
            EnablePeriodicEvent = false;
            _continuTimerLoop = false;
            if (_timerThread != null && _timerThread.IsAlive) _timerThread.Abort();
        }

        public T Dequeue()
        {
            lock (_syncRoot)
            {
                T item = First;
                RemoveAt(0);
                return item;
            }
        }

        public void Enqueue(T item)
        {
            lock (_syncRoot)
            {
                Add(item);
            }
        }

        public bool Exists(T item)
        {
            lock (_syncRoot)
            {
                foreach (T t in this)
                {
                    if (t.CompareTo(item) == 0) return true;
                }
                return false;
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Starts or abort the periodic thread
        /// </summary>
        public bool EnablePeriodicEvent
        {
            get
            {
                lock (_syncRoot)
                {
                    return _enablePeriodicEvent;
                }
            }
            set
            {
                lock (_syncRoot)
                {
                    _enablePeriodicEvent = value;
                }
            }
        }

        public bool AutoDequeueOnEvent
        {
            get
            {
                lock (_syncRoot)
                {
                    return _autoDequeueOnEvent;
                }
            }
            set
            {
                lock (_syncRoot)
                {
                    _autoDequeueOnEvent = value;
                }
            }
        }

        public T First
        {
            get
            {
                lock (_syncRoot)
                {
                    return this[0];
                }
            }
        }

        #endregion

        #region Events

        public event QueueThreadedPeriodicEventHandler OnQueueThreadedPeriodicEvent;

        #endregion

        #region Private Methods

        private void TimerThreadStart()
        {
            while (_continuTimerLoop)
            {
                Thread.Sleep(_periodicInterval);
                if (Count > 0 && EnablePeriodicEvent)
                {
                    if (OnQueueThreadedPeriodicEvent != null) OnQueueThreadedPeriodicEvent(First);
                    if (AutoDequeueOnEvent) Dequeue();
                }
            }
        }

        #endregion

        #region Fields

        private bool _enablePeriodicEvent;
        private bool _autoDequeueOnEvent;
        private bool _continuTimerLoop;
        private readonly int _periodicInterval;
        private readonly object _syncRoot = new object();
        private readonly Thread _timerThread;

        #endregion

        public delegate void QueueThreadedPeriodicEventHandler(T firstItem);
    }
}