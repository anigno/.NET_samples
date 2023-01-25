using System;
using System.Collections.Generic;
using System.Linq;

namespace AnignoraDataTypes.Lists
{
    /// <summary>
    /// Extends a generic List, Adding filtering items
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ListFiltered<T> : List<T>
    {
        #region Constructors

        public ListFiltered(Func<T, bool> p_condition)
        {
            Filter = p_condition;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Add item only if condition is met
        /// </summary>
        /// <returns>true if item was added, else false</returns>
        public bool AddFiltered(T p_item)
        {
            if (Filter(p_item))
            {
                Add(p_item);
                FilteredItemAdded(this, new ListFilteredEventArgs<T>(p_item));
                return true;
            }
            return false;
        }

        public bool InsertFiltered(int p_index, T p_item)
        {
            if (Filter(p_item))
            {
                Insert(p_index, p_item);
                FilteredItemAdded(this, new ListFilteredEventArgs<T>(p_item));
                return true;
            }
            return false;
        }

        /// <summary>
        /// Verify all existing items setisfy the condition, if not remove them
        /// </summary>
        /// <returns>Removed items</returns>
        public T[] FilterExistingItems()
        {
            T[] removedItems = this.Where(p_item => !Filter(p_item)).ToArray();
            removedItems.DoForAll(
                p_item =>
                {
                    Remove(p_item);
                    FilteredItemRemove(this, new ListFilteredEventArgs<T>(p_item));
                });
            return removedItems;
        }

        #endregion

        #region Public Properties

        public Func<T, bool> Filter { get; private set; }

        #endregion

        #region Events

        public event EventHandler<ListFilteredEventArgs<T>> FilteredItemAdded = delegate { };
        public event EventHandler<ListFilteredEventArgs<T>> FilteredItemRemove = delegate { };

        #endregion
    }
}