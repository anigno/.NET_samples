#region

using System.Collections.Generic;
using System.Windows;

#endregion

namespace MarkupExamples
{
    public class ViewModelBinder : DependencyObject
    {
        #region Public Methods

        public static string GetViewModelKey(DependencyObject p_obj)
        {
            return (string) p_obj.GetValue(ViewModelKeyProperty);
        }

        public static void SetViewModelKey(DependencyObject p_obj, string p_value)
        {
            p_obj.SetValue(ViewModelKeyProperty, p_value);
        }

        public static void AddViewModel(string p_name, object p_viewModelInstance)
        {
            s_viewModelDictionary.Add(p_name, p_viewModelInstance);
        }

        #endregion

        #region Private Methods

        private static void onViewModelKeyChange(DependencyObject p_d, DependencyPropertyChangedEventArgs p_e)
        {
            ValueSource source = DependencyPropertyHelper.GetValueSource(p_d, ViewModelKeyProperty);
            if (source.BaseValueSource != BaseValueSource.Local) return;
            FrameworkElement element = p_d as FrameworkElement;
            if (element == null) return;
            string viewModelKey = (string) p_e.NewValue;
            //DependencyObject dependencyObject = new DependencyObject();
            //bool isInDesignMode = DesignerProperties.GetIsInDesignMode(dependencyObject);
            //if (isInDesignMode || viewModelKey == string.Empty) return;
            if (!s_viewModelDictionary.ContainsKey(viewModelKey)) return;
            element.DataContext = s_viewModelDictionary[viewModelKey];
        }

        #endregion

        #region Fields

        public static readonly DependencyProperty ViewModelKeyProperty = DependencyProperty.RegisterAttached("ViewModelKey", typeof (string), typeof (ViewModelBinder), new FrameworkPropertyMetadata(string.Empty, onViewModelKeyChange));

        private static readonly Dictionary<string, object> s_viewModelDictionary = new Dictionary<string, object>();

        #endregion
    }
}