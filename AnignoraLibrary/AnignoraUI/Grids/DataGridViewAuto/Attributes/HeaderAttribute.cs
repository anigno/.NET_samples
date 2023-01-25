using System;

namespace AnignoraUI.Grids.DataGridViewAuto.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class HeaderAttribute : Attribute
    {
        public HeaderAttribute(string p_header, int p_width = 60)
        {
            ColumnHeader = p_header;
            ColumnWidth = p_width;
        }

        public string ColumnHeader { get; set; }
        public string ColumnName { get; set; }
        public int ColumnWidth { get; set; }
    }
}