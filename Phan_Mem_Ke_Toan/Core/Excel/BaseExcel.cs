using Microsoft.Office.Interop.Excel;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Phan_Mem_Ke_Toan.Core.Excel
{
    internal class ExcelApplication
    {
        private static readonly Lazy<Application> application = new Lazy<Application>(() => new Application());
        public static Application Application { get => application.Value; }
    }

    internal abstract class BaseExcel : IDisposable
    {
        protected Application excel = null;
        protected Workbook workbook = null;
        protected Worksheet sheet = null;
        protected string template = null;
        protected bool isReadOnly = false;

        protected void InitExcel()
        {
            excel = ExcelApplication.Application;
            workbook = excel.Workbooks.Open(template);
            sheet = excel.ActiveSheet as Worksheet;
        }

        public void NewSheets(string[] sheetsName)
        {
            if (sheetsName != null)
            {
                Sheets xlSheets = workbook.Sheets;
                for (int index = 0; index < sheetsName.Length; index++)
                {
                    sheet.Copy(Type.Missing, xlSheets[xlSheets.Count]);
                    xlSheets[xlSheets.Count].Name = sheetsName[index];
                }
            }
        }

        public void FocusToSheetName(string sheetName)
        {
            sheet = excel.Sheets[sheetName];
        }

        protected string GetExcelColumnName(int columnNumber)
        {
            string columnName = "";

            while (columnNumber > 0)
            {
                int modulo = (columnNumber - 1) % 26;
                columnName = Convert.ToChar('A' + modulo) + columnName;
                columnNumber = (columnNumber - modulo) / 26;
            }

            return columnName;
        }

        public void ShowExcel()
        {
            if (excel != null && sheet != null)
            {
                try
                {
                    //sheet.Protect("doanxempassword");
                    excel.Visible = true;
                    excel.EditDirectlyInCell = false;
                }
                catch
                {

                }
            }
        }

        public void ShowPrintPreview()
        {
            if (excel != null && sheet != null)
            {
                try
                {
                    sheet.PageSetup.Zoom = false;
                    sheet.PageSetup.PaperSize = XlPaperSize.xlPaperA3;
                    sheet.PageSetup.FitToPagesTall = 1;
                    sheet.PrintPreview();
                }
                catch
                {

                }
            }
        }

        public void Dispose()
        {
            try
            {
                workbook.Close();
                excel.Quit();
            }
            catch
            {

            }
            //Do it when app closed
            /*finally
            {
                if (sheet != null)
                    Marshal.ReleaseComObject(sheet);
                if (workbook != null)
                    Marshal.ReleaseComObject(workbook);
                if (excel != null)
                    Marshal.ReleaseComObject(excel);
                excel = null;
            }*/

            GC.SuppressFinalize(this);
        }

        protected int FindLastRowUsed()
        {
            return sheet.Cells.Find("*", System.Reflection.Missing.Value,
                               System.Reflection.Missing.Value, System.Reflection.Missing.Value,
                               XlSearchOrder.xlByColumns, XlSearchDirection.xlPrevious,
                               false, System.Reflection.Missing.Value, System.Reflection.Missing.Value).Row;
        }

        protected int FindLastColumnUsed()
        {
            return sheet.Cells.Find("*", System.Reflection.Missing.Value,
                               System.Reflection.Missing.Value, System.Reflection.Missing.Value,
                               XlSearchOrder.xlByColumns, XlSearchDirection.xlPrevious,
                               false, System.Reflection.Missing.Value, System.Reflection.Missing.Value).Column;
        }
    }
}
