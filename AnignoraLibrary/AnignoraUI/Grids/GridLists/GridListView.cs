using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace AnignoraUI.Grids.GridLists
{
    public class GridListView : DataGridView
    {
		#region (------  Constructors  ------)

        #endregion (------  Constructors  ------)

		#region (------  Overridden Methods  ------)

        protected override void OnBackgroundColorChanged(EventArgs e)
        {
            base.OnBackgroundColorChanged(e);
            ColumnHeadersDefaultCellStyle.BackColor = BackgroundColor;
            DefaultCellStyle.BackColor = BackgroundColor;
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            RowHeadersVisible = false;
            EnableHeadersVisualStyles = false;
            AllowUserToAddRows = false;
            AllowUserToDeleteRows = false;
            EditMode = DataGridViewEditMode.EditProgrammatically;
            ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            Rows.Clear();
            Columns.Clear();
        }

        protected override void OnGridColorChanged(EventArgs e)
        {
            base.OnGridColorChanged(e);
            ColumnHeadersDefaultCellStyle.ForeColor = GridColor;
            DefaultCellStyle.ForeColor = GridColor;
        }

		#endregion (------  Overridden Methods  ------)

		#region (------  Public Methods  ------)

        /// <summary>
        /// Adds columns, column name and column key are the same
        /// </summary>
        /// <param name="columnsNames"></param>
        public void AddColumns(params string[] columnsNames)
        {
            foreach (string columnName in columnsNames)
            {
                Columns.Add(columnName, columnName);
                Columns[columnName].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        /// <summary>
        /// Adds columns, column name and column key are the same
        /// </summary>
        /// <param name="columnNameAndWidth"></param>
        public void AddColumns(Dictionary<string, int> columnNameAndWidth)
        {
            AddColumns(columnNameAndWidth.Keys.ToArray());
            SetColumnsWidth(columnNameAndWidth.Values.ToArray());
        }

        public DataGridViewRow AddRow(params string[] cellsTexts)
        {
            int i = Rows.Add(cellsTexts);
            return Rows[i];
        }

        public DataGridViewRow GetRow(int columnIndex, string cellValue)
        {
            string columnName = Columns[columnIndex].Name;
            return GetRow(columnName, cellValue);
        }

        public DataGridViewRow GetRow(string columnName,string cellValue)
        {
            foreach (DataGridViewRow row in Rows)
            {
                if(row.Cells[columnName].Value.ToString()==cellValue)
                {
                    return row;
                }
            }
            return null;
        }

        /// <summary>
        /// Auto add columns according to AutoColumn attribute on the reflected object type
        /// </summary>
        /// <param name="reflectedType"></param>
        /// <param name="clearExistingColumns">If true, clears any existing columns. else add new columns</param>
        public void InitColumnsUsingReflection(Type reflectedType,bool clearExistingColumns)
        {
            Rows.Clear();
            if(clearExistingColumns)Columns.Clear();
            MemberInfo[] memberInfoArray = reflectedType.GetMembers(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
            SortedDictionary<int,DataGridViewColumn> requestedColumns=new SortedDictionary<int, DataGridViewColumn>();
            foreach (MemberInfo memberInfo in memberInfoArray)
            {
                object[] customAttributes=memberInfo.GetCustomAttributes(false);
                //Prepare requested columns sorted by column index
                foreach (object attributeObject in customAttributes)
                {
                    if(attributeObject is AutoColumnAttribute)
                    {
                        string varName = memberInfo.Name;
                        int columnIndex = ((AutoColumnAttribute) attributeObject).ColumnIndex;
                        string columnName = ((AutoColumnAttribute)attributeObject).ColumnName;
                        string columntext = ((AutoColumnAttribute)attributeObject).ColumnText;
                        int columnWidth = ((AutoColumnAttribute)attributeObject).ColumnWidth;
                        DataGridViewColumn column = new DataGridViewColumn
                                                        {
                                                            Name = columnName,
                                                            HeaderText = columntext,
                                                            Width = columnWidth,
                                                            Tag = varName
                                                        };
                        requestedColumns.Add(columnIndex,column);
                        //if (columnIndex > Columns.Count) columnIndex = Columns.Count ;
                        //Columns.Insert(columnIndex,column);
                    }
                }
            }
            foreach(DataGridViewColumn column in requestedColumns.Values)
            {
                Columns.Add(column);
            }
        }

        public void SetCellColors(DataGridViewCell cell, Color backColor, Color foreColor)
        {
            cell.Style.BackColor = backColor;
            cell.Style.ForeColor = foreColor;
            cell.Style.SelectionBackColor = foreColor;
            cell.Style.SelectionForeColor = backColor;
        }

        public void SetCellColors(DataGridViewRow row, string columnName, Color backColor, Color foreColor)
        {
            DataGridViewCell cell = row.Cells[columnName];
            SetCellColors(cell, backColor, foreColor);
        }

        public void SetColumnColors(int i, Color backColor, Color foreColor)
        {
            DataGridViewColumn column = Columns[i];
            SetColumnColors(column, backColor, foreColor);
        }

        public void SetColumnColors(string columnName, Color backColor, Color foreColor)
        {
            DataGridViewColumn column = Columns[columnName];
            SetColumnColors(column, backColor, foreColor);
        }

        public void SetColumnColors(DataGridViewColumn column, Color backColor, Color foreColor)
        {
            column.DefaultCellStyle.BackColor = backColor;
            column.DefaultCellStyle.ForeColor = foreColor;
            column.DefaultCellStyle.SelectionBackColor = foreColor;
            column.DefaultCellStyle.SelectionForeColor = backColor;
        }

        public void SetColumnsWidth(params int[] widths)
        {
            for(int a=0;a<Columns.Count;a++)
            {
                Columns[a].Width = widths[a];
            }
        }

        public void UpdateRow(DataGridViewRow row,params string[] cellsTexts)
        {
            for(int a=0;a<cellsTexts.Length;a++)
            {
                row.Cells[a].Value = cellsTexts[a];
            }
        }

		#endregion (------  Public Methods  ------)
    }
}
