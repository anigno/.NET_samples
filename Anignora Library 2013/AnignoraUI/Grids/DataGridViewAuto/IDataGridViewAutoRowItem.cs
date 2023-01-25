namespace AnignoraUI.Grids.DataGridViewAuto
{
    public interface IDataGridViewAutoRowItem
    {
        /// <summary>
        /// Returns an array of values as it would be displayed as a grid row, must keep order as given by ColumnHeader attribute's index property
        /// </summary>
        object[] GetValuesArray();
    }
}