using OfficeOpenXml;
using OfficeOpenXml.Table.PivotTable;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmaduraLosaRevit.Model.TablasSchedule.Usos
{
    public interface IPivotTableCreator
    {
        void CreatePivotTable(
            OfficeOpenXml.ExcelPackage pkg, // reference to the destination book
            string tableName,               // "tab" name used to generate names for related items
            string pivotRangeName);         // Named range in the Workbook refers to data
    }


    // refereicnia :https://es.coredump.biz/questions/11650080/epplus-pivot-tablescharts
    public class SimplePivotTable : IPivotTableCreator
    {
        List<string> _GroupByColumns;
        List<string> _SummaryColumns;
        /// <summary>
        /// Constructor
        /// </summary>
        public SimplePivotTable(string[] groupByColumns, string[] summaryColumns)
        {
            _GroupByColumns = new List<string>(groupByColumns);
            _SummaryColumns = new List<string>(summaryColumns);
        }

        /// <summary>
        /// Call-back handler that builds simple PivatTable in Excel
        /// http://stackoverflow.com/questions/11650080/epplus-pivot-tables-charts
        /// </summary>
        public void CreatePivotTable(OfficeOpenXml.ExcelPackage pkg, string tableName, string pivotRangeName)
        {
            string pageName = "Pivot-" + tableName.Replace(" ", "");
            var wsPivot = pkg.Workbook.Worksheets.Add(pageName);
            pkg.Workbook.Worksheets.MoveBefore(pageName, tableName);


            /*
             ExcelWorkSheet.Cells[FromRow, FromCol, ToRow, ToCol]

            ws.Cells[1,1,1,10]  // toma todas las celdas desde primera fila y primera columna(A1), hasta primera fila y columna 10 ( A10)
             
             */

            // toma toda la hoja
            var dataRange = pkg.Workbook./*Worksheets[tableName].*/
             Names[pivotRangeName];
            var pivotTable = wsPivot.PivotTables.Add(wsPivot.Cells["C3"], dataRange, "Pivot_" + tableName.Replace(" ", ""));
            pivotTable.ShowHeaders = true;
            pivotTable.UseAutoFormatting = true;
            pivotTable.ApplyWidthHeightFormats = true;
            pivotTable.ShowDrill = true;
            pivotTable.FirstHeaderRow = 1;  // first row has headers
            pivotTable.FirstDataCol = 1;    // first col of data
            pivotTable.FirstDataRow = 2;    // first row of data

            foreach (string row in _GroupByColumns)
            {
                var field = pivotTable.Fields[row];
                pivotTable.RowFields.Add(field);
                field.Sort = eSortType.Ascending;
            }

            foreach (string column in _SummaryColumns)
            {
                var field = pivotTable.Fields[column];
                ExcelPivotTableDataField result = pivotTable.DataFields.Add(field);
            }

            pivotTable.DataOnRows = false;
        }


        /*
        public void ejemplosimple()
        {
            DataTable table = getDataSource();
            FileInfo fileInfo = new FileInfo(path);
            var excel = new ExcelPackage(fileInfo);
            var wsData = excel.Workbook.Worksheets.Add("Data-Worksheetname");
            var wsPivot = excel.Workbook.Worksheets.Add("Pivot-Worksheetname");
            wsData.Cells["A1"].LoadFromDataTable(table, true, OfficeOpenXml.Table.TableStyles.Medium6);
            if (table.Rows.Count != 0)
            {
                foreach (DataColumn col in table.Columns)
                {
                    // format all dates in german format (adjust accordingly)
                    if (col.DataType == typeof(System.DateTime))
                    {
                        var colNumber = col.Ordinal + 1;
                        var range = wsData.Cells[2, colNumber, table.Rows.Count + 1, colNumber];
                        range.Style.Numberformat.Format = "dd.MM.yyyy";
                    }
                }
            }

            var dataRange = wsData.Cells[wsData.Dimension.Address.ToString()];
            dataRange.AutoFitColumns();
            var pivotTable = wsPivot.PivotTables.Add(wsPivot.Cells["A3"], dataRange, "Pivotname");
            pivotTable.MultipleFieldFilters = true;
            pivotTable.RowGrandTotals = true;
            pivotTable.ColumGrandTotals = true;
            pivotTable.Compact = true;
            pivotTable.CompactData = true;
            pivotTable.GridDropZones = false;
            pivotTable.Outline = false;
            pivotTable.OutlineData = false;
            pivotTable.ShowError = true;
            pivotTable.ErrorCaption = "[error]";
            pivotTable.ShowHeaders = true;
            pivotTable.UseAutoFormatting = true;
            pivotTable.ApplyWidthHeightFormats = true;
            pivotTable.ShowDrill = true;
            pivotTable.FirstDataCol = 3;
            pivotTable.RowHeaderCaption = "Claims";

            var modelField = pivotTable.Fields["Model"];
            pivotTable.PageFields.Add(modelField);
            modelField.Sort = OfficeOpenXml.Table.PivotTable.eSortType.Ascending;

            var countField = pivotTable.Fields["Claims"];
            pivotTable.DataFields.Add(countField);

            var countryField = pivotTable.Fields["Country"];
            pivotTable.RowFields.Add(countryField);
            var gspField = pivotTable.Fields["GSP / DRSL"];
            pivotTable.RowFields.Add(gspField);

            var oldStatusField = pivotTable.Fields["Old Status"];
            pivotTable.ColumnFields.Add(oldStatusField);
            var newStatusField = pivotTable.Fields["New Status"];
            pivotTable.ColumnFields.Add(newStatusField);

            var submittedDateField = pivotTable.Fields["Claim Submitted Date"];
            pivotTable.RowFields.Add(submittedDateField);
            submittedDateField.AddDateGrouping(OfficeOpenXml.Table.PivotTable.eDateGroupBy.Months | OfficeOpenXml.Table.PivotTable.eDateGroupBy.Days);
            var monthGroupField = pivotTable.Fields.GetDateGroupField(OfficeOpenXml.Table.PivotTable.eDateGroupBy.Months);
            monthGroupField.ShowAll = false;
            var dayGroupField = pivotTable.Fields.GetDateGroupField(OfficeOpenXml.Table.PivotTable.eDateGroupBy.Days);
            dayGroupField.ShowAll = false;

            excel.Save();
        }
        */

    }

}
