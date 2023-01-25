using System;

namespace AnignoraDataTypes.Lists
{
    public class ListFilteredEventArgs<T> : EventArgs
    {
        #region Constructors

        public ListFilteredEventArgs(T p_item)
        {
            Item = p_item;
        }

        #endregion

        #region Public Properties

        public T Item { get; private set; }

        #endregion
    }
}