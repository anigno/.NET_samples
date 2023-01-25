#region

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

#endregion

namespace WpfDataGridCustomTest.Converters
{
    public class NumberEnumNumberStringTranslator
    {
        #region Constructors

        static NumberEnumNumberStringTranslator()
        {
            NumberEnumToStringDictionary.Add(NumberEnum.EnumOne, "One String");
            NumberEnumToStringDictionary.Add(NumberEnum.EnumTwo, "Two String");
            NumberEnumToStringDictionary.Add(NumberEnum.EnumThree, "Three String");
            NumberEnumToStringDictionary.Add(NumberEnum.EnumFour, "Four String");
        }

        #endregion

        #region Fields

        public static readonly Dictionary<NumberEnum, string> NumberEnumToStringDictionary = new Dictionary<NumberEnum, string>();

        #endregion
    }

    public class NumberEnumToNumberStringConverter : IValueConverter
    {
        #region Public Methods

        public object Convert(object p_value, Type p_targetType, object p_parameter, CultureInfo p_culture)
        {
            NumberEnum numberEnum = (NumberEnum) p_value;
            string numberString = NumberEnumNumberStringTranslator.NumberEnumToStringDictionary[numberEnum];
            return numberString;
        }

        public object ConvertBack(object p_value, Type p_targetType, object p_parameter, CultureInfo p_culture)
        {
            string val = (string) p_value;
            NumberEnum numberEnum = NumberEnumNumberStringTranslator.NumberEnumToStringDictionary.Single(p_pair => p_pair.Value == val).Key;
            return numberEnum;
        }

        #endregion
    }

    public class NumberEnumsToNumberStringsConverter : IValueConverter
    {
        #region Public Methods

        public object Convert(object p_value, Type p_targetType, object p_parameter, CultureInfo p_culture)
        {
            ObservableCollection<NumberEnum> numberEnums = (ObservableCollection<NumberEnum>) p_value;
            IEnumerable<string> numbersStrings = numberEnums.Select(p_numberEnum => NumberEnumNumberStringTranslator.NumberEnumToStringDictionary[p_numberEnum]);
            return numbersStrings;
        }

        public object ConvertBack(object p_value, Type p_targetType, object p_parameter, CultureInfo p_culture)
        {
            return DependencyProperty.UnsetValue;
        }

        #endregion
    }
}