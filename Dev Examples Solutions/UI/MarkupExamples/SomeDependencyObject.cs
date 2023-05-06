#region

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

#endregion

namespace MarkupExamples
{
    public class SomeDependencyObject : DependencyObject
    {
        #region Constructors

        static SomeDependencyObject()
        {
            FrameworkPropertyMetadata frameworkPropertyMetadata = new FrameworkPropertyMetadata(string.Empty, onSomePropertyChange);
            SomeProperty = DependencyProperty.RegisterAttached("SomeProperty", typeof(string), typeof(SomeDependencyObject), frameworkPropertyMetadata);
        }

        #endregion

        #region Public Methods

        public static string GetSomeProperty(DependencyObject p_obj)
        {
            return (string)p_obj.GetValue(SomeProperty);
        }

        public static void SetSomeProperty(DependencyObject p_obj, string p_value)
        {
            p_obj.SetValue(SomeProperty, p_value);
        }

        #endregion

        #region Private Methods

        private static void onSomePropertyChange(DependencyObject p_d, DependencyPropertyChangedEventArgs p_e)
        {
            string s = (string)p_e.NewValue;
            Control w = p_d as Control;
            w.Background = Brushes.Blue;
            w.Foreground = Brushes.Yellow;
            Label l = p_d as Label;
            if(l==null)return;
            l.Content = s;
        }

        #endregion

        #region Fields

        public static readonly DependencyProperty SomeProperty;

        #endregion
    }
}