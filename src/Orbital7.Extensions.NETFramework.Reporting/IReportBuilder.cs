using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Orbital7.Extensions.NETFramework.Reporting
{
    public enum ReportFormat
    {
        Pdf,

        Native,
    }

    public interface IReportBuilder
    {
        string GetFileExtension(ReportFormat reportFormat);

        string GetContentType(ReportFormat reportFormat);

        byte[] Save(ReportFormat reportFormat);

        byte[] CreatePngPreview();

        void WriteError(Exception ex);
    }
}
