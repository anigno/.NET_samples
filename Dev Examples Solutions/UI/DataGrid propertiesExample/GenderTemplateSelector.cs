#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;

#endregion

namespace DataGrid
{
    public class GenderTemplateSelector : DataTemplateSelector
    {
        #region Public Methods

        public override DataTemplate SelectTemplate(object item,
            DependencyObject container)
        {
            var customer = item as Customer;
            if (customer == null)
                return base.SelectTemplate(item, container);

            if (customer.Gender == Gender.Male)
            {
                return MaleTemplate;
            }
            return FemaleTemplate;
        }

        #endregion

        #region Public Properties

        public DataTemplate MaleTemplate { get; set; }
        public DataTemplate FemaleTemplate { get; set; }

        #endregion
    }
}