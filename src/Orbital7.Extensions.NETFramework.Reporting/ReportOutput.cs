using System;
using System.Collections.Generic;
using System.Text;

namespace Orbital7.Extensions.NETFramework.Reporting
{
    public class ReportOutput
    {
        public string Filename { get; set; }

        public string FileContentType { get; set; }

        public byte[] FileContents { get; set; }

        public byte[] PreviewPngContents { get; set; }

        public ReportOutput()
        {

        }

        public ReportOutput(IReport report, IReportBuilder reportBuilder, ReportFormat reportFormat, bool includePreview)
        {
            this.Filename = report.GetFilename(reportBuilder.GetFileExtension(reportFormat));
            this.FileContentType = reportBuilder.GetContentType(reportFormat);
            this.FileContents = reportBuilder.Save(reportFormat);
            if (includePreview)
                this.PreviewPngContents = reportBuilder.CreatePngPreview();
        }

        public override string ToString()
        {
            return this.Filename;
        }
    }
}
