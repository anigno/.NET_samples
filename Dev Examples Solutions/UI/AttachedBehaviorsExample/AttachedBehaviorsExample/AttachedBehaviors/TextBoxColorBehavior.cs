#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

#endregion

namespace AttachedBehaviorsExample.AttachedBehaviors
{
    public class TextBoxColorBehavior
    {
        #region Public Methods

        public static bool GetIsSetColorByContent(TextBox p_textBox)
        {
            return (bool) p_textBox.GetValue(IsSetColorByContentProperty);
        }

        public static void SetIsSetColorByContent(TextBox p_textBox, bool p_value)
        {
            p_textBox.SetValue(IsSetColorByContentProperty, p_value);
        }

        #endregion

        #region Private Methods

        private static void OnIsSetColorByContentChanged(DependencyObject p_d, DependencyPropertyChangedEventArgs p_e)
        {
            TextBox item = p_d as TextBox;
            if (item == null) return;
            if (p_e.NewValue is bool == false) return;
            if ((bool) p_e.NewValue)
                item.TextChanged += item_TextChanged;
            else
                item.TextChanged -= item_TextChanged;
        }

        private static void item_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!object.ReferenceEquals(sender, e.OriginalSource)) return;
            TextBox item = e.OriginalSource as TextBox;
            if (item == null) return;
            item.Background = new SolidColorBrush(Colors.Green);
            if (item.Text != "") item.Background = new SolidColorBrush(Colors.Blue);
        }

        #endregion

        #region Fields

        public static readonly DependencyProperty IsSetColorByContentProperty =
            DependencyProperty.RegisterAttached("IsSetColorByContent",
                typeof (bool),
                typeof (TextBoxColorBehavior),
                new UIPropertyMetadata(false, OnIsSetColorByContentChanged));

        #endregion
    }
}