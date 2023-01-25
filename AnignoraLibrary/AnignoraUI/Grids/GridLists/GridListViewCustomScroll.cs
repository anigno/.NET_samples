using System;
using System.Drawing;
using System.Windows.Forms;

namespace AnignoraUI.Grids.GridLists
{
    public partial class GridListViewCustomScroll : UserControl
    {
		#region (------  Constructors  ------)

        public GridListViewCustomScroll()
        {
            InitializeComponent();
        }

		#endregion (------  Constructors  ------)

		#region (------  Public Methods  ------)

        public void AddColumns(params string[] columnsNames)
        {
            foreach(string columnName in columnsNames)
            {
                dataGridViewMain.Columns.Add(columnName, columnName);
                trackBarHorizontal.Maximum = dataGridViewMain.Columns.Count-1;
            }
        }

        public DataGridViewRow AddRow(params string[] cellsTexts)
        {
            trackBarVertical.Maximum = dataGridViewMain.Rows.Count - 1;
            if (trackBarVertical.Value + 1 <= trackBarVertical.Maximum) trackBarVertical.Value++;
            int i=dataGridViewMain.Rows.Add(cellsTexts);
            return dataGridViewMain.Rows[i];
        }

        public void SetCellColors(DataGridViewCell cell, Color backColor, Color foreColor)
        {
            cell.Style.BackColor = backColor;
            cell.Style.ForeColor = foreColor;
            cell.Style.SelectionBackColor = foreColor;
            cell.Style.SelectionForeColor = backColor;
        }

        public void SetCellColors(DataGridViewRow row,string columnName, Color backColor, Color foreColor)
        {
            DataGridViewCell cell = row.Cells[columnName];
            SetCellColors(cell, backColor, foreColor);
        }

        public void SetColumnColors(int i,Color backColor,Color foreColor)
        {
            DataGridViewColumn column=dataGridViewMain.Columns[i];
            SetColumnColors(column, backColor, foreColor);
        }

        public void SetColumnColors(string columnName, Color backColor, Color foreColor)
        {
            DataGridViewColumn column = dataGridViewMain.Columns[columnName];
            SetColumnColors(column,backColor,foreColor);
        }

        public void SetColumnColors(DataGridViewColumn column, Color backColor, Color foreColor)
        {
            column.DefaultCellStyle.BackColor = backColor;
            column.DefaultCellStyle.ForeColor = foreColor;
            column.DefaultCellStyle.SelectionBackColor = foreColor;
            column.DefaultCellStyle.SelectionForeColor = backColor;
        }

		#endregion (------  Public Methods  ------)

		#region (------  Protected Methods  ------)

        protected override void OnBackColorChanged(EventArgs e)
        {
            base.OnBackColorChanged(e);
            dataGridViewMain.BackColor = BackColor;
            trackBarVertical.BackColor = BackColor;
            trackBarHorizontal.BackColor = BackColor;
            dataGridViewMain.ColumnHeadersDefaultCellStyle.BackColor = BackColor;
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            trackBarHorizontal.ValueChanged += new EventHandler(trackBarHorizontal_ValueChanged);
            trackBarVertical.ValueChanged += new EventHandler(trackBarVertical_ValueChanged);
            dataGridViewMain.GotFocus += new EventHandler(dataGridViewMain_GotFocus);
        }

        protected override void OnForeColorChanged(EventArgs e)
        {
            base.OnForeColorChanged(e);
            dataGridViewMain.ForeColor = ForeColor;
            dataGridViewMain.ColumnHeadersDefaultCellStyle.ForeColor = ForeColor;
            dataGridViewMain.GridColor = ForeColor;
            trackBarVertical.ForeColor = ForeColor;
            trackBarHorizontal.ForeColor = ForeColor;
        }

		#endregion (------  Protected Methods  ------)

		#region (------  Private Methods  ------)

        void dataGridViewMain_GotFocus(object sender, EventArgs e)
        {
            trackBarVertical.Focus();
        }

        void trackBarHorizontal_ValueChanged(object sender, EventArgs e)
        {
            if (dataGridViewMain.Columns.Count < 2) return;
            dataGridViewMain.FirstDisplayedScrollingColumnIndex = trackBarHorizontal.Value;
        }

        void trackBarVertical_ValueChanged(object sender, EventArgs e)
        {
            if(dataGridViewMain.Rows.Count<2)return;
            dataGridViewMain.FirstDisplayedScrollingRowIndex = trackBarVertical.Maximum - trackBarVertical.Value ;
        }

		#endregion (------  Private Methods  ------)
    }
}
