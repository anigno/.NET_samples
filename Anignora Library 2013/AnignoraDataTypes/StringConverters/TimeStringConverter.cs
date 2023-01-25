using System;
using System.ComponentModel;
using System.Globalization;

namespace AnignoraDataTypes.StringConverters
{
    public class TimeStringConverter : StringConverter
    {
        #region Public Methods

        public override bool CanConvertTo(ITypeDescriptorContext p_context, Type p_destinationType)
        {
            if (p_destinationType == typeof (DateTime)) return true;
            return base.CanConvertTo(p_context, p_destinationType);
        }

        public override bool CanConvertFrom(ITypeDescriptorContext p_context, Type p_sourceType)
        {
            if (p_sourceType == typeof (string)) return true;
            return base.CanConvertFrom(p_context, p_sourceType);
        }

        public override object ConvertTo(ITypeDescriptorContext p_context, CultureInfo p_culture, object p_value, Type p_destinationType)
        {
            if (p_destinationType == typeof (String) && p_value is DateTime)
            {
                return ((DateTime) p_value).ToString("hh:mm");
            }
            return base.ConvertTo(p_context, p_culture, p_value, p_destinationType);
        }

        public override object ConvertFrom(ITypeDescriptorContext p_context, CultureInfo p_culture, object p_value)
        {
            if (p_value is string)
            {
                try
                {
                    string s = p_value as string;
                    string[] sa = s.Split(new[] {";"}, StringSplitOptions.RemoveEmptyEntries);
                    return sa;
                }
                catch
                {
                    throw new ArgumentException(string.Format("Can not convert '{0}' to string array", p_value));
                }
            }
            return base.ConvertFrom(p_context, p_culture, p_value);
        }

        #endregion
    }
}