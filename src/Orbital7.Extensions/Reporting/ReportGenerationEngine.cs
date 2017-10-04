using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbital7.Extensions.Reporting
{
    public class ReportGenerationEngine
    {
        public async Task<ReportOutput> CreateReportAsync<T>(ReportBase<T> report,
            T reportBuilder, ReportFormat reportFormat, bool includePreview, bool writeBuildErrorToReport)
            where T : IReportBuilder
        {
            try
            {
                await report.BuildAsync(reportBuilder);
            }
            catch (Exception ex)
            {
                if (writeBuildErrorToReport)
                    reportBuilder.WriteError(ex);
                else
                    throw ex;
            }

            return new ReportOutput(report, reportBuilder, reportFormat, includePreview);
        }
    }
}
