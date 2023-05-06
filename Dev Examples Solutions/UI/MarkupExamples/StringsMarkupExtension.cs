#region

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Markup;

#endregion

namespace MarkupExamples
{
    public class StringsMarkupExtension : MarkupExtension
    {
        #region Constructors

        public StringsMarkupExtension()
        {
            MyStrings.Add("Added in CTor");
        }

        #endregion

        #region Public Methods

        public override object ProvideValue(IServiceProvider p_serviceProvider)
        {
            MyStrings.Add("Added in ProvideValue()");
            return MyStrings.ToArray();
        }

        #endregion

        #region Fields

        public static ObservableCollection<string> MyStrings = new ObservableCollection<string>();

        #endregion
    }
}