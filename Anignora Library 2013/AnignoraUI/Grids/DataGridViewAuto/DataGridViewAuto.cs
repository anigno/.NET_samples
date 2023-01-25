using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using AnignoraUI.Grids.DataGridViewAuto.Attributes;

namespace AnignoraUI.Grids.DataGridViewAuto
{
    public class DataGridViewAuto : DataGridView
    {
		#region (------  Fields  ------)
        private readonly Dictionary<string, CellAlignmentAttribute> m_alignAttributes = new Dictionary<string, CellAlignmentAttribute>();
        private readonly List<HeaderAttribute> m_columnHeaderAttributes = new List<HeaderAttribute>();
        private readonly Dictionary<string, List<RangeColorAttribute>> m_rangeColorAttributes = new Dictionary<string, List<RangeColorAttribute>>();
        private readonly Dictionary<string, List<StringColorAttribute>> m_stringColorAttributes = new Dictionary<string, List<StringColorAttribute>>();
        private readonly Dictionary<string, StringFormatAttribute> m_stringFormatAttributes = new Dictionary<string,
            StringFormatAttribute>();
        private readonly Dictionary<string, List<StringHideAttribute>> m_stringHideAttributes = new Dictionary<string, List<StringHideAttribute>>();
		#endregion (------  Fields  ------)

		#region (------  Constructors  ------)
        public DataGridViewAuto()
        {
            RowHeadersVisible = false;
            AllowUserToAddRows = false;
            AllowUserToDeleteRows = false;
            SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            EditMode = DataGridViewEditMode.EditProgrammatically;
            ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            AllowUserToResizeRows = false;
            AllowUserToOrderColumns = true;
        }
		#endregion (------  Constructors  ------)

		#region (------  Public Methods  ------)
        public void RemoveRow(object p_keyValue,Func<IDataGridViewAutoRowItem, object> p_rowKeyFunc)
        {
            DataGridViewRow rowToRemove=null;
            foreach (DataGridViewRow row in Rows)
            {
                object vl = p_rowKeyFunc((IDataGridViewAutoRowItem) row.Tag);
                object v2 = p_keyValue;
                if (vl.Equals(v2))
                {
                    rowToRemove = row;
                    break;
                }
            }
            if (rowToRemove != null) Rows.Remove(rowToRemove);
        }

        /// <summary>
        /// Add or update existing row, row is searched according to given function III </summary>
        /// <param name="p_dataGridViewAutoRowItem">Item to update existing one</param>
        /// <param name="p_rowKeyFunc">Function to serch existing rows, comparing same cell in given IGridAutoObject and in all rows's IGridAutoObject</param>
        public void AddUpdateRow(IDataGridViewAutoRowItem p_dataGridViewAutoRowItem, Func<IDataGridViewAutoRowItem, object> p_rowKeyFunc)
        {
            foreach (DataGridViewRow row in Rows)
            {
                object vl = p_rowKeyFunc((IDataGridViewAutoRowItem) row.Tag);
                object v2 = p_rowKeyFunc(p_dataGridViewAutoRowItem);
                if (vl.Equals(v2))
                {
                    //update
                    updateRow(row.Index, p_dataGridViewAutoRowItem);
                    return;
                }
            }
            //Add
            addRow(p_dataGridViewAutoRowItem);
        }

        public void SetColumnsVisible(bool p_visible, params string[] p_columnsNames)
        {
            foreach (string columnsName in p_columnsNames)
            {
                Columns[columnsName].Visible = p_visible;
            }
        }

        /// <summary>
        /// Initialize grid and add columns headers according to given type III </summary>
        /// <param name="p_type">Type that represent a row, must have ColumnHeaderAttributes, must implement IGridAutoObject</param>
        public void SetGridRowItemType(Type p_type)
        {
            PropertyInfo[] propertylnfoArray = p_type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (PropertyInfo propertyInfo in propertylnfoArray)
            {
                //Create empty lists for properties attributes
                m_stringFormatAttributes[propertyInfo.Name] = new StringFormatAttribute("");
                m_rangeColorAttributes[propertyInfo.Name] = new List<RangeColorAttribute>();
                m_stringColorAttributes[propertyInfo.Name] = new List<StringColorAttribute>();
                m_stringHideAttributes[propertyInfo.Name] = new List<StringHideAttribute>();
                foreach (Attribute attribute in Attribute.GetCustomAttributes(propertyInfo))
                {
                    if (attribute is HeaderAttribute)
                    {
                        HeaderAttribute headerAttribute = (HeaderAttribute) attribute;
                        headerAttribute.ColumnName = propertyInfo.Name;
                        m_columnHeaderAttributes.Add(headerAttribute);
                    }
                    if (attribute is StringFormatAttribute)
                    {
                        StringFormatAttribute StringFormatAttribute = (StringFormatAttribute) attribute;
                        m_stringFormatAttributes[propertyInfo.Name] = StringFormatAttribute;
                    }
                    if (attribute is CellAlignmentAttribute)
                    {
                        CellAlignmentAttribute CellAlignmentAttribute = (CellAlignmentAttribute) attribute;
                        m_alignAttributes[propertyInfo.Name] = CellAlignmentAttribute;
                    }
                    if (attribute is RangeColorAttribute)
                    {
                        RangeColorAttribute rangeColorAttribute = (RangeColorAttribute) attribute;
                        m_rangeColorAttributes[propertyInfo.Name].Add(rangeColorAttribute);
                    }
                    if (attribute is StringColorAttribute)
                    {
                        StringColorAttribute stringColorAttribute = (StringColorAttribute)attribute;
                        m_stringColorAttributes[propertyInfo.Name].Add(stringColorAttribute);
                    }
                    if (attribute is StringHideAttribute)
                    {
                        StringHideAttribute stringHideAttribute = (StringHideAttribute)attribute;
                        m_stringHideAttributes[propertyInfo.Name].Add(stringHideAttribute);
                    }
                }
            }
            //Create the columns 
            Columns.Clear();
            foreach (HeaderAttribute columnHeaderAttribute in m_columnHeaderAttributes)
            {
                DataGridViewColumn column = new DataGridViewColumn(new DataGridViewTextBoxCell());
                column.Name = columnHeaderAttribute.ColumnName;
                column.HeaderText = columnHeaderAttribute.ColumnHeader;
                column.Width = columnHeaderAttribute.ColumnWidth;
                if (m_alignAttributes.ContainsKey(column.Name))
                    column.CellTemplate.Style.Alignment = m_alignAttributes[column.Name].Alignment;
                else
                    column.CellTemplate.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                Columns.Add(column);
            }
        }

        public void SetSelection(IDataGridViewAutoRowItem p_dataGridViewAutoRowItem, Func<IDataGridViewAutoRowItem, object> p_rowKeyFunc)
        {
            ClearSelection();
            foreach (DataGridViewRow row in Rows)
            {
                var vl = p_rowKeyFunc((IDataGridViewAutoRowItem) row.Tag);
                var v2 = p_rowKeyFunc(p_dataGridViewAutoRowItem);
                if (vl.Equals(v2))
                {
                    SetSelectedRowCore(row.Index, true);
                    return;
                }
            }
        }

        public void SortByColumn(int p_index, ListSortDirection p_direction)
        {
            if (p_index >= Columns.Count) return;
            Sort(Columns[p_index], p_direction);
        }

        private void updateRow(int p_rowIndex, IDataGridViewAutoRowItem p_dataGridViewAutoRowItem)
        {
            DataGridViewRow row = Rows[p_rowIndex];
            row.Tag = p_dataGridViewAutoRowItem;
            object[] newValues = p_dataGridViewAutoRowItem.GetValuesArray();
            for (int celllndex = 0; celllndex < row.Cells.Count; celllndex++)
            {
                string columnlMame = row.Cells[celllndex].OwningColumn.Name;
                //StringFormatAttribute
                string format = m_stringFormatAttributes[columnlMame].StringFormat;
                string formattedValue = string.Format(string.Format("{{0:{0}}}", format), newValues[celllndex]);
                row.Cells[celllndex].Value = formattedValue;
                row.Cells[celllndex].Style.ForeColor = row.DefaultCellStyle.ForeColor;
                row.Cells[celllndex].Style.BackColor = row.DefaultCellStyle.BackColor;
                row.Cells[celllndex].Style.SelectionForeColor = row.DefaultCellStyle.SelectionForeColor;
                row.Cells[celllndex].Style.SelectionBackColor = row.DefaultCellStyle.SelectionBackColor;
                //RangeColorAttribute
                List<RangeColorAttribute> rangeColorAttributes = m_rangeColorAttributes[columnlMame];
                foreach (RangeColorAttribute rangeColorAttribute in rangeColorAttributes)
                {
                    object o = newValues[celllndex];
                    double d = Convert.ToDouble(o);
                    if (d >= rangeColorAttribute.MinRange && d <= rangeColorAttribute.MaxRange)
                    {
                        row.Cells[celllndex].Style.ForeColor = Color.FromArgb(rangeColorAttribute.ForeColor);
                        row.Cells[celllndex].Style.BackColor = Color.FromArgb(rangeColorAttribute.BackColor);
                        row.Cells[celllndex].Style.SelectionForeColor = row.Cells[celllndex].Style.ForeColor; //row.Cells[cellIndex].Style.SelectionBackColor = row.Cells[cellIndex].Style.ForeColor; //row.Cells[cellIndex].Style.SelectionForeColor = row.Cells[cellIndex].Style.BackColor;
                    }
                }
                //StringColorAttribute
                List<StringColorAttribute> stringColorAttribute = m_stringColorAttributes[columnlMame];
                foreach (StringColorAttribute stringValueAttribute in stringColorAttribute)
                {
                    string s = row.Cells[celllndex].Value.ToString();
                    if (stringValueAttribute.ConditionlsEqual == (s == stringValueAttribute.StringValue))
                    {
                        row.Cells[celllndex].Style.ForeColor = Color.FromArgb(stringValueAttribute.ForeColor);
                        row.Cells[celllndex].Style.BackColor = Color.FromArgb(stringValueAttribute.BackColor);
                        row.Cells[celllndex].Style.SelectionForeColor = row.Cells[celllndex].Style.ForeColor; //row.Cells[cellIndex].Style.SelectionBackColor = row.Cells[cellIndex].Style. ForeColor; //row.Cells[cellIndex].Style.SelectionForeColor = row.Cells[cellIndex].Style.BackColor;
                    }
                }
                //StringHideAttribute
                List<StringHideAttribute> stringHideAttributes = m_stringHideAttributes[columnlMame];
                foreach (StringHideAttribute stringHideAttribute in stringHideAttributes)
                {
                    string s = row.Cells[celllndex].Value.ToString();
                    if (s == stringHideAttribute.StringValue)
                    {
                        row.Cells[celllndex].Value = "";
                        //row.Cells[celllndex].Style.ForeColor = row.Cells[celllndex].Style.BackColor ;
                        //row.Cells[celllndex].Style.SelectionForeColor = row.Cells[celllndex].Style.SelectionBackColor;
                    }
                }
                row.Cells[celllndex].ToolTipText = row.Cells[celllndex].Value.ToString();
            }
        }
		#endregion (------  Public Methods  ------)

		#region (------  Private Methods  ------)
        private void addRow(IDataGridViewAutoRowItem p_dataGridViewAutoRowItem)
        {
            DataGridViewRow newRow = new DataGridViewRow();
            newRow.Tag = p_dataGridViewAutoRowItem;
            int i = Rows.Add(newRow);
            updateRow(i, p_dataGridViewAutoRowItem);
        }
		#endregion (------  Private Methods  ------)
    }
}
