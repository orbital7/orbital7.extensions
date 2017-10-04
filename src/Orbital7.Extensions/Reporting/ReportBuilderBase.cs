using System;
using System.Collections.Generic;
using System.Text;

namespace Orbital7.Extensions.Reporting
{
    public abstract class ReportBuilderBase : IReportBuilder
    {
        protected abstract byte[] CreatePngPreview();

        protected abstract string GetContentType(ReportFormat reportFormat);

        protected abstract string GetFileExtension(ReportFormat reportFormat);

        protected abstract byte[] Save(ReportFormat reportFormat);

        protected abstract void WriteError(Exception ex);

        protected virtual string GetFilename(IReport report, ReportFormat reportFormat)
        {
            return report.GetFilename(this.GetFileExtension(reportFormat));
        }

        byte[] IReportBuilder.CreatePngPreview()
        {
            return CreatePngPreview();
        }

        string IReportBuilder.GetContentType(ReportFormat reportFormat)
        {
            return GetContentType(reportFormat);
        }

        string IReportBuilder.GetFileExtension(ReportFormat reportFormat)
        {
            return GetFileExtension(reportFormat);
        }

        string IReportBuilder.GetFilename(IReport report, ReportFormat reportFormat)
        {
            return GetFilename(report, reportFormat);
        }

        byte[] IReportBuilder.Save(ReportFormat reportFormat)
        {
            return Save(reportFormat);
        }

        void IReportBuilder.WriteError(Exception ex)
        {
            WriteError(ex);
        }
    }
}
