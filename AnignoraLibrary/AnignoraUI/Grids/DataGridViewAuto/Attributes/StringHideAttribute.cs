using System;

namespace AnignoraUI.Grids.DataGridViewAuto.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class StringHideAttribute : Attribute
    {
        public StringHideAttribute(string p_stringValue)
        {
            StringValue = p_stringValue;
        }

        public string StringValue { get; set; }
    }
}