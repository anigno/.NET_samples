using System;

namespace AnignoraUI.Grids.DataGridViewAuto.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class RangeColorAttribute : Attribute
    {
        public RangeColorAttribute(float p_minRange, float p_maxRange, int p_foreColor, int p_backColor)
        {
            MinRange = p_minRange;
            MaxRange = p_maxRange;
            ForeColor = p_foreColor;
            BackColor = p_backColor;
        }

        public int BackColor { get; set; }
        public int ForeColor { get; set; }
        public float MaxRange { get; set; }
        public float MinRange { get; set; }
    }
}