using System;
using System.Collections.Generic;
using System.Text;
using GemBox.Spreadsheet;
using System.IO;

namespace Orbital7.Extensions.Reporting.GemBox
{
    public abstract class GemBoxSpreadsheetReportBuilderBase : ReportBuilderBase
    {
        private const string SHEET1 = "Sheet1";

        protected ExcelFile ExcelFile { get; set; }

        public GemBoxSpreadsheetReportBuilderBase(string licenseKey)
        {
            SpreadsheetInfo.SetLicense(licenseKey);
            this.ExcelFile = new ExcelFile();
        }

        protected override byte[] CreatePngPreview()
        {
            this.ExcelFile.Worksheets.ActiveWorksheet = this.ExcelFile.Worksheets[0];

            var saveOptions = new ImageSaveOptions()
            {
                SelectionType = SelectionType.ActiveSheet,
                Format = ImageSaveFormat.Png,
                PageNumber = 0,
                PageCount = 1,
            };

            using (var ms = new MemoryStream())
            {
                this.ExcelFile.Save(ms, saveOptions);
                return ms.ToArray();
            }
        }

        protected override string GetContentType(ReportFormat reportFormat)
        {
            if (reportFormat == ReportFormat.Pdf)
                return MimeTypesHelper.MIME_TYPE_PDF;
            else if (reportFormat == ReportFormat.Csv)
                return MimeTypesHelper.MIME_TYPE_CSV;
            else
                return MimeTypesHelper.MIME_TYPE_XLSX;
        }

        protected override string GetFileExtension(ReportFormat reportFormat)
        {
            if (reportFormat == ReportFormat.Pdf)
                return MimeTypesHelper.FILE_EXT_PDF;
            else if (reportFormat == ReportFormat.Csv)
                return MimeTypesHelper.FILE_EXT_CSV;
            else
                return MimeTypesHelper.FILE_EXT_XLSX;
        }

        protected override byte[] Save(ReportFormat reportFormat)
        {
            SaveOptions saveOptions = null;
            if (reportFormat == ReportFormat.Pdf)
                saveOptions = new PdfSaveOptions() { SelectionType = SelectionType.EntireFile };
            else if (reportFormat == ReportFormat.Csv)
                saveOptions = SaveOptions.CsvDefault;
            else
                saveOptions = SaveOptions.XlsxDefault;

            using (var ms = new MemoryStream())
            {
                this.ExcelFile.Save(ms, saveOptions);
                return ms.ToArray();
            }
        }

        protected override void WriteError(Exception ex)
        {
            for (int index = this.ExcelFile.Worksheets.Count - 1; index >= 0; index--)
                this.ExcelFile.Worksheets.Remove(index);

            CreateTextWorksheet("ERROR: " + ex.Message + ex.StackTrace, "ERROR");
        }
        
        public virtual ExcelWorksheet CreateTextWorksheet(string text, string worksheetName = SHEET1, bool portrait = false, 
            double margins = 0.5)
        {
            var ws = CreateWorksheet(worksheetName, portrait, true, margins);
            ws.Cells[0, 0].Value = text;
            ws.Columns[0].AutoFit(1, ws.Rows[0], ws.Rows[0]);

            return ws;
        }

        public virtual ExcelWorksheet CreateWorksheet(string worksheetName = SHEET1, bool portrait = false, 
            bool fitToSinglePage = true, double margins = 0.5)
        {
            if (String.IsNullOrEmpty(worksheetName))
                worksheetName = SHEET1;
            else
                worksheetName = worksheetName.Replace(":", "").Replace("\\", "").Replace("/", "").Replace("?", "")
                                             .Replace("*", "").Replace("[", "").Replace("]", "").PruneEnd("'");

            worksheetName = worksheetName.EnsureMaxStringLength(31);

            var ws = this.ExcelFile.Worksheets.Add(worksheetName);
            if (fitToSinglePage)
            {
                ws.PrintOptions.FitToPage = true;
                ws.PrintOptions.FitWorksheetWidthToPages = 1;
            }
            ws.PrintOptions.Portrait = portrait;
            ws.PrintOptions.BottomMargin = margins;
            ws.PrintOptions.LeftMargin = margins;
            ws.PrintOptions.RightMargin = margins;
            ws.PrintOptions.TopMargin = margins;

            return ws;
        }
    }
}
