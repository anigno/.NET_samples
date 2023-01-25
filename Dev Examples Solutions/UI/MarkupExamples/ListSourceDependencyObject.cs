using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace MarkupExamples
{
    public class ListSourceDependencyObject : DependencyObject
    {
        #region Constructors

        static ListSourceDependencyObject()
        {
            FrameworkPropertyMetadata frameworkPropertyMetadata = new FrameworkPropertyMetadata(string.Empty, onSomePropertyChange);
            SomeProperty = DependencyProperty.RegisterAttached("ListSource", typeof (string), typeof (ListSourceDependencyObject), frameworkPropertyMetadata);
        }

        #endregion

        #region Public Methods

        public static string GetLisrSource(DependencyObject p_obj)
        {
            return (string) p_obj.GetValue(SomeProperty);
        }

        public static void SetListSource(DependencyObject p_obj, string p_value)
        {
            p_obj.SetValue(SomeProperty, p_value);
        }

        #endregion

        #region Private Methods

        private static void onSomePropertyChange(DependencyObject p_d, DependencyPropertyChangedEventArgs p_e)
        {
            string s = (string) p_e.NewValue;
            ItemsControl itemsControl = p_d as ItemsControl;
            if (itemsControl == null) return;
            if (s.ToUpper() == "NAMES")
            {
                itemsControl.Items.Add("Avi");
                itemsControl.Items.Add("Beni");
                itemsControl.Items.Add("Carrie");
            }
            else if (s.ToUpper() == "ANIMALS")
            {
                itemsControl.Items.Add("Dog");
                itemsControl.Items.Add("Cat");
                itemsControl.Items.Add("Lion");
                itemsControl.Items.Add("Monkey");
            }
        }

        #endregion

        #region Fields

        public static readonly DependencyProperty SomeProperty;

        #endregion
    }
}