using System;

namespace AnignoraUI.Grids.DataGridViewAuto.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class StringColorAttribute : Attribute
    {
        public StringColorAttribute(string p_stringValue, int p_foreColor, int p_backColor, bool p_conditionIsEqual)
        {
            StringValue = p_stringValue;
            ForeColor = p_foreColor;
            BackColor = p_backColor;
            ConditionlsEqual = p_conditionIsEqual;
        }

        public bool ConditionlsEqual { get; set; }
        public int BackColor { get; set; }
        public int ForeColor { get; set; }
        public string StringValue { get; set; }
    }
}