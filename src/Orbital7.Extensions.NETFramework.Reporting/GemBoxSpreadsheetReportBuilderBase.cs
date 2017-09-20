using System;
using System.Collections.Generic;
using System.Text;
using GemBox.Spreadsheet;
using System.IO;

namespace Orbital7.Extensions.NETFramework.Reporting
{
    public abstract class GemBoxSpreadsheetReportBuilderBase : IReportBuilder
    {
        private const string SHEET1 = "Sheet1";

        protected ExcelFile ExcelFile { get; set; }

        protected ExcelWorksheet ActiveWorksheet { get; set; }

        public GemBoxSpreadsheetReportBuilderBase(string licenseKey)
        {
            SpreadsheetInfo.SetLicense(licenseKey);
            this.ExcelFile = new ExcelFile();
        }

        string IReportBuilder.GetFileExtension(ReportFormat reportFormat)
        {
            if (reportFormat == ReportFormat.Pdf)
                return ".pdf";
            else
                return ".xlsx";
        }

        string IReportBuilder.GetContentType(ReportFormat reportFormat)
        {
            if (reportFormat == ReportFormat.Pdf)
                return "application/pdf";
            else
                return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        }

        byte[] IReportBuilder.Save(ReportFormat reportFormat)
        {
            SaveOptions saveOptions = null;
            if (reportFormat == ReportFormat.Pdf)
                saveOptions = new PdfSaveOptions() { SelectionType = SelectionType.EntireFile };
            else
                saveOptions = SaveOptions.XlsxDefault;

            using (var ms = new MemoryStream())
            {
                this.ExcelFile.Save(ms, saveOptions);
                return ms.ToArray();
            }
        }

        byte[] IReportBuilder.CreatePngPreview()
        {
            throw new NotImplementedException();
        }

        void IReportBuilder.WriteError(Exception ex)
        {
            for (int index = this.ExcelFile.Worksheets.Count - 1; index >= 0; index--)
                this.ExcelFile.Worksheets.Remove(index);

            CreateTextWorksheet("ERROR: " + ex.Message + ex.StackTrace, "ERROR");
        }

        public virtual ExcelWorksheet CreateTextWorksheet(string text, string worksheetName = SHEET1, bool portrait = false, 
            double margins = 0.5)
        {
            CreateWorksheet(worksheetName, portrait, margins);
            this.ActiveWorksheet.Cells[0, 0].Value = text;
            this.ActiveWorksheet.Columns[0].AutoFit(1, this.ActiveWorksheet.Rows[0], this.ActiveWorksheet.Rows[0]);

            return this.ActiveWorksheet;
        }

        public virtual ExcelWorksheet CreateWorksheet(string worksheetName = SHEET1, bool portrait = false, 
            double margins = 0.5)
        {
            if (String.IsNullOrEmpty(worksheetName))
                worksheetName = SHEET1;
            else
                worksheetName = worksheetName.Replace(":", "").Replace("\\", "").Replace("/", "").Replace("?", "")
                                             .Replace("*", "").Replace("[", "").Replace("]", "").PruneEnd("'");

            this.ActiveWorksheet = this.ExcelFile.Worksheets.Add(worksheetName);
            this.ActiveWorksheet.PrintOptions.FitToPage = true;
            this.ActiveWorksheet.PrintOptions.FitWorksheetWidthToPages = 1;
            this.ActiveWorksheet.PrintOptions.Portrait = portrait;

            this.ActiveWorksheet.PrintOptions.BottomMargin = margins;
            this.ActiveWorksheet.PrintOptions.LeftMargin = margins;
            this.ActiveWorksheet.PrintOptions.RightMargin = margins;
            this.ActiveWorksheet.PrintOptions.TopMargin = margins;

            return this.ActiveWorksheet;
        }
    }
}
