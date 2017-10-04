using System;
using System.Collections.Generic;
using System.Text;

namespace Orbital7.Extensions.Reporting.GemBox
{
    public abstract class GemBoxDocumentReportBuilderBase : ReportBuilderBase
    {
        public GemBoxDocumentReportBuilderBase(string licenseKey)
        {
            // TODO.
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
            throw new NotImplementedException();
        }

        protected override byte[] CreatePngPreview()
        {
            throw new NotImplementedException();
        }

        protected override void WriteError(Exception ex)
        {
            throw new NotImplementedException();
        }
    }
}
