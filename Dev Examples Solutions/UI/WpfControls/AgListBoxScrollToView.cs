#region

using System.Collections.Specialized;
using System.Windows.Controls;

#endregion

namespace WpfControls
{
    public class AgListBoxScrollToView : ListView
    {
        #region Constructors

        public AgListBoxScrollToView()
        {
            ((INotifyCollectionChanged) Items).CollectionChanged += ListBoxScrollView_CollectionChanged;
        }

        #endregion

        #region Private Methods

        private void ListBoxScrollView_CollectionChanged(object p_sender, NotifyCollectionChangedEventArgs p_e)
        {
            if (Items.Count == 0) return;
            var lastItem = Items[Items.Count - 1];
            ScrollIntoView(lastItem);
        }

        #endregion
    }
}