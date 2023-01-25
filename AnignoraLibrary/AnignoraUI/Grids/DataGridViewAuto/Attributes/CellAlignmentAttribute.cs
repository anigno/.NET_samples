using System;
using System.Windows.Forms;

namespace AnignoraUI.Grids.DataGridViewAuto.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class CellAlignmentAttribute : Attribute
    {
        public CellAlignmentAttribute(DataGridViewContentAlignment p_dataGridViewContentAlignment)
        {
            Alignment = p_dataGridViewContentAlignment;
        }

        public DataGridViewContentAlignment Alignment { get; set; }
    }
}