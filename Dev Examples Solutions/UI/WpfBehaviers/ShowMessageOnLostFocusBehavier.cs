#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

#endregion

namespace WpfBehaviers
{
    public class ShowMessageOnLostFocusBehavier : Behavior<TextBox>
    {
        #region Protected Methods

        protected override void OnAttached()
        {
            AssociatedObject.LostFocus += AssociatedObject_LostFocus;
            base.OnAttached();
        }

        protected override void OnDetaching()
        {
            AssociatedObject.LostFocus -= AssociatedObject_LostFocus;
            base.OnDetaching();
        }

        #endregion

        #region Private Methods

        private void AssociatedObject_LostFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            Control c = e.Source as Control;
            MessageBox.Show(c.Name + " Lost focus");
        }

        #endregion
    }
}