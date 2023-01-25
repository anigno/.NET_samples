using System;

namespace AnignoraUI.Grids.DataGridViewAuto.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class StringFormatAttribute : Attribute
    {
        public StringFormatAttribute(string p_stringFormat)
        {
            StringFormat = p_stringFormat;
        }

        public string StringFormat { get; set; }
    }
}