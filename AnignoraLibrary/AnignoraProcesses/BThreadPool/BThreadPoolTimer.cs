using System;
using System.Threading;
using AnignoraCommonAndHelpers;

namespace AnignoraProcesses.BThreadPool
{
    public class BThreadPoolTimer : IStartable, IDisposable
    {
		#region (------  Fields  ------)
        private Action<object> m_callBack;
        private object m_callbackParam;
        private readonly Timer m_timer;
		#endregion (------  Fields  ------)

		#region (------  Constructors  ------)
        public BThreadPoolTimer(Action<object> p_callBack, int p_dueTime, int p_period)
        {
            m_timer = new Timer(timerCallBack);
            m_callBack = p_callBack;
            DueTime = p_dueTime;
            Period = p_period;
        }

        public BThreadPoolTimer(Action<object> p_callBack)
            : this(p_callBack, int.MaxValue, int.MaxValue)
        {
        }
		#endregion (------  Constructors  ------)

		#region (------  Properties  ------)
        public int DueTime { get; private set; }

        public int Period { get; private set; }
		#endregion (------  Properties  ------)

		#region (------  Public Methods  ------)
        public void Change(Action<object> p_action, object p_callbackParam, int p_dueTime, int p_period)
        {
            Stop();
            m_callBack = p_action;
            m_callbackParam = p_callbackParam;
            DueTime = p_dueTime;
            Period = p_period;
            Start();
        }

        public void Dispose()
        {
            m_timer.Change(int.MaxValue, int.MaxValue);
            m_timer.Dispose();
        }

        public void Start()
        {
            m_timer.Change(DueTime, Period);
        }

        public void Stop()
        {
            m_timer.Change(int.MaxValue, int.MaxValue);
        }
		#endregion (------  Public Methods  ------)

		#region (------  Private Methods  ------)
        private void timerCallBack(object p_timerStateObject)
        {
            if (m_callbackParam == null) m_callbackParam = this;
            m_callBack(m_callbackParam);
        }
		#endregion (------  Private Methods  ------)
    }
}