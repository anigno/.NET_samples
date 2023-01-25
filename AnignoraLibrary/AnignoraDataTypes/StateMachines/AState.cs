using System.Collections.Generic;

namespace AnignoraDataTypes.StateMachines
{
    public class AState<T>
    {
        #region Constructors

        public AState(string p_name, bool p_isFinal = false)
        {
            Name = p_name;
            IsFinal = p_isFinal;
            PostActions = new List<StateAction<T>>();
            PreActions = new List<StateAction<T>>();
            Conditions = new List<StateCondition<T>>();
        }

        #endregion

        #region Public Methods

        public AState<T> GetNextState(T p_data)
        {
            AState<T> nextState = null;
            foreach (StateAction<T> action in PreActions)
            {
                action(p_data);
            }
            foreach (StateCondition<T> condition in Conditions)
            {
                nextState = condition(p_data);
                if (nextState != null)
                {
                    break;
                }
            }
            //Couldn't met any condition, stay is this state
            if (nextState == null) nextState = this;
            foreach (StateAction<T> action in PostActions)
            {
                action(p_data);
            }
            return nextState;
        }

        public override string ToString()
        {
            return Name;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// List of state conditions, where each condition can return a next state if a condition is met or null III </summary>
        public List<StateCondition<T>> Conditions { get; private set; }

        public bool IsFinal { get; private set; }

        public string Name { get; private set; }

        /// <summary>
        /// Action performed before checking conditions III </summary>
        public List<StateAction<T>> PostActions { get; private set; }

        /// <summary>
        /// Action performed after checking conditions and before moving to next state if any
        /// </summary>
        public List<StateAction<T>> PreActions { get; private set; }

        #endregion

        public delegate void StateAction<in TD>(TD p_data);

        public delegate AState<TD> StateCondition<TD>(TD p_data);
    }
}