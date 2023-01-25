using System;

namespace AnignoraUI.Grids.GridLists
{
    /// <summary>
    /// GridListView attribute for auto adding columns from a type,used with InitColumnsUsingReflection()
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class AutoColumnAttribute : Attribute
    {
        public string ColumnName { get; private set; }
        public string ColumnText { get; private set; }
        public int ColumnWidth { get; private set; }
        public int ColumnIndex { get; private set; } 

        public AutoColumnAttribute(int columnIndex,string columnName,string columnText,int columnWidth=120)
        {
            ColumnName = columnName;
            ColumnText = columnText;
            ColumnWidth = columnWidth;
            ColumnIndex = columnIndex;
        }

    }
}
