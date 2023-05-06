#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

#endregion

namespace WpfTreeViewExamples.Library
{
    public class Utility
    {
        #region Public Methods

        public static UIElement GetChildControl(DependencyObject parentObject, string childName)
        {
            UIElement element = null;

            if (parentObject != null)
            {
                int totalChild = VisualTreeHelper.GetChildrenCount(parentObject);
                for (int i = 0; i < totalChild; i++)
                {
                    DependencyObject childObject = VisualTreeHelper.GetChild(parentObject, i);

                    if (childObject is FrameworkElement && ((FrameworkElement) childObject).Name == childName)
                    {
                        element = childObject as UIElement;
                        break;
                    }

                    // get its child 

                    element = GetChildControl(childObject, childName);
                    if (element != null) break;
                }
            }

            return element;
        }

        #endregion
    }
}