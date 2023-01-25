#region

using System;

#endregion

namespace States.StateMachine
{
    public class AStateMachine<T>
    {
        #region Constructors

        public AStateMachine(AState<T> p_rootState)
        {
            m_rootState = p_rootState;
            m_currentState = m_rootState;
        }

        #endregion

        #region Public Methods

        public AState<T> NextState(T p_data)
        {
            foreach (Func<T, AState<T>> trigger in m_currentState.Triggers)
            {
                AState<T> triggerResultState = trigger(p_data);
                //Check for no state or same state
                if (triggerResultState == null) continue;
                if (triggerResultState == m_currentState) continue;
                //Other state
                AState<T> prevState = m_currentState;
                m_currentState = triggerResultState;
                StateChanged(this, new StateChangedEventArgs<T>(prevState, m_currentState, p_data));
                prevState.StateLeaveAction(p_data);
                m_currentState.StateEnterAction(p_data);
                //Check for finit state
                if (m_currentState.IsFinit) StateFinit(this, new StateChangedEventArgs<T>(prevState, m_currentState, p_data));
                return triggerResultState;
            }
            return m_currentState;
        }

        public void MoveToRoot()
        {
            AState<T> tempCurrent = m_currentState;
            m_currentState = m_rootState;
            StateChanged(this, new StateChangedEventArgs<T>(tempCurrent, m_rootState, default(T)));
        }

        #endregion

        #region Events

        public event EventHandler<StateChangedEventArgs<T>> StateChanged = delegate { };
        public event EventHandler<StateChangedEventArgs<T>> StateFinit = delegate { };

        #endregion

        #region Fields

        private readonly AState<T> m_rootState;
        private AState<T> m_currentState;

        #endregion
    }
}