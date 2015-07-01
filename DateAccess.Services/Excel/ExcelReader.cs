using System;
using System.Reflection;
using Microsoft.Office.Interop.Excel;

namespace DateAccess.Services.Excel
{
    internal static class ExcelReader
    {
        public static object[,] Read(string path, string sheetName, string rangeStart, string rangeEnd)
        {
            var app = new Application();

            var book = app.Workbooks.Open(path);
            var sheet = (Worksheet)book.Sheets[sheetName];
            var range = sheet.Range[rangeStart, rangeEnd];
            var values = (object[,])range.Value2;
            
            range = null;
            sheet = null;
            book.Close(false, Missing.Value, Missing.Value);
            book = null;
            app.Quit();
            app = null;

            return values;
        }
    }
}
