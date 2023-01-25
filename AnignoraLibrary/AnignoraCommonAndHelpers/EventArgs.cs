using System;

namespace AnignoraCommonAndHelpers
{
    public class EventArgs<T> : EventArgs
    {
        #region Constructors

        public EventArgs(T p_t)
        {
            Item = p_t;
        }

        #endregion

        #region Public Properties

        public T Item { get; private set; }

        #endregion
    }
}