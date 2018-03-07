using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using GemBox.Document;

namespace Orbital7.Extensions.Reporting.GemBox
{
    public abstract class GemBoxDocumentReportBuilderBase : ReportBuilderBase
    {
        protected DocumentModel WordDocument { get; set; }

        public GemBoxDocumentReportBuilderBase(string licenseKey)
        {
            ComponentInfo.SetLicense(licenseKey);
            this.WordDocument = new DocumentModel();
        }

        protected override string GetContentType(ReportFormat reportFormat)
        {
            if (reportFormat == ReportFormat.Pdf)
                return "application/pdf";
            else
                return "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
        }

        protected override string GetFileExtension(ReportFormat reportFormat)
        {
            if (reportFormat == ReportFormat.Pdf)
                return ".pdf";
            else
                return ".docx";
        }

        protected override byte[] Save(ReportFormat reportFormat)
        {
            using (var ms = new MemoryStream())
            {
                this.WordDocument.Save(ms, (reportFormat == ReportFormat.Native) ?
                    (SaveOptions)new DocxSaveOptions() : new PdfSaveOptions());
                return ms.ToArray();
            }
        }

        protected override byte[] CreatePngPreview()
        {
            using (var ms = new MemoryStream())
            {
                this.WordDocument.Save(ms, new ImageSaveOptions()
                {
                    Format = ImageSaveFormat.Png,
                    PageNumber = 0,
                    PageCount = 1,

                });
                return ms.ToArray();
            }
        }

        protected override void WriteError(Exception ex)
        {
            this.WordDocument = new DocumentModel();
            this.WordDocument.Sections.Add(new Section(this.WordDocument,
                new Paragraph(this.WordDocument, "ERROR: " + ex.Message + ex.StackTrace)));
        }
    }
}
