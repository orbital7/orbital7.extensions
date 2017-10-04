using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Orbital7.Extensions.Reporting
{
    public enum ReportFormat
    {
        Pdf,

        Native,
    }

    public interface IReportBuilder
    {
        string GetFilename(IReport report, ReportFormat reportFormat);

        string GetFileExtension(ReportFormat reportFormat);

        string GetContentType(ReportFormat reportFormat);

        byte[] Save(ReportFormat reportFormat);

        byte[] CreatePngPreview();

        void WriteError(Exception ex);
    }
}
