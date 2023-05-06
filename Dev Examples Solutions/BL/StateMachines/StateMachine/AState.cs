#region

using System;
using System.Collections.Generic;

#endregion

namespace States.StateMachine
{
    public class AState<T>
    {
        #region Constructors

        public AState(string p_name, bool p_isFinit)
        {
            Triggers = new List<Func<T, AState<T>>>();
            Description = "ND";
            Name = p_name;
            IsFinit = p_isFinit;
            StateEnterAction = delegate { };
            StateLeaveAction = delegate { };
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return string.Format("[{0}] [{1}] [{2}]", Name, Description, IsFinit);
        }

        #endregion

        #region Public Properties

        public List<Func<T, AState<T>>> Triggers { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public bool IsFinit { get; private set; }

        public Action<T> StateEnterAction { get; set; }
        public Action<T> StateLeaveAction { get; set; }

        #endregion
    }
}