using System.Collections.Generic;
//using Microsoft.Office.Interop.Excel;

namespace AnignoraCommonAndHelpers.Helpers
{
    public class OfficeAutomationHelper
    {
        //public static string[][] ReadExcelFile(string p_workbookPath, string p_currentSheet,string p_rangeStart,string p_rangeEnd)
        //{
        //    List<string[]> lRet = new List<string[]>();
        //    Application excelApp = new Application();
        //    excelApp.Visible = true;
        //    Workbook excelWorkbook = excelApp.Workbooks.Open(p_workbookPath, 0, false, 5, "", "", false, XlPlatform.xlWindows, "", true, false, 0, true, false, false);
        //    Sheets excelSheets = excelWorkbook.Worksheets;
        //    Worksheet excelWorksheet = (Worksheet) excelSheets.get_Item(p_currentSheet);
        //    Range excelRange = excelWorksheet.get_Range(p_rangeStart, p_rangeEnd);
        //    int totalColumns = excelRange.Columns.Count;
        //    int totalRows = excelRange.Rows.Count;
        //    for (int r = 1; r <= totalRows; r++)
        //    {
        //        string[] sa = new string[totalColumns];
        //        for (int c = 1; c <= totalColumns; c++)
        //        {
        //            object o = ((Range) excelRange.get_Item(r, c)).Value2;
        //            string v = o == null ? "" : o.ToString();
        //            sa[c-1] = v;
        //        }
        //        lRet.Add(sa);
        //    }
        //    excelApp.Workbooks.Close();
        //    return lRet.ToArray();
        //}
    }
}