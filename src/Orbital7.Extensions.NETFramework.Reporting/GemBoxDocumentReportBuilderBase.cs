using System;
using System.Collections.Generic;
using System.Text;

namespace Orbital7.Extensions.NETFramework.Reporting
{
    public abstract class GemBoxDocumentReportBuilderBase : IReportBuilder
    {
        public GemBoxDocumentReportBuilderBase(string licenseKey)
        {
            // TODO.
        }

        byte[] IReportBuilder.CreatePngPreview()
        {
            throw new NotImplementedException();
        }

        string IReportBuilder.GetContentType(ReportFormat reportFormat)
        {
            if (reportFormat == ReportFormat.Pdf)
                return "application/pdf";
            else
                return "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
        }

        string IReportBuilder.GetFileExtension(ReportFormat reportFormat)
        {
            if (reportFormat == ReportFormat.Pdf)
                return ".pdf";
            else
                return ".docx";
        }

        byte[] IReportBuilder.Save(ReportFormat reportFormat)
        {
            throw new NotImplementedException();
        }

        void IReportBuilder.WriteError(Exception ex)
        {
            throw new NotImplementedException();
        }
    }
}
