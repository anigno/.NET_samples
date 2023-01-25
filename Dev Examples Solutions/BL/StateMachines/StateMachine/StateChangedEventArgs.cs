#region

using System;

#endregion

namespace States.StateMachine
{
    public class StateChangedEventArgs<T> : EventArgs
    {
        #region Constructors

        public StateChangedEventArgs(AState<T> p_prevState, AState<T> p_newStateState, T p_data)
        {
            PrevState = p_prevState;
            NewState = p_newStateState;
            Data = p_data;
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return string.Format("Prev=[{0}] New=[{1}] Data=[{2}]", PrevState, NewState, Data);
        }

        #endregion

        #region Public Properties

        public AState<T> PrevState { get; private set; }
        public AState<T> NewState { get; private set; }
        public T Data { get; private set; }

        #endregion
    }
}