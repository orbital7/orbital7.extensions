using System;
using System.Collections.Generic;
using System.Text;

namespace Orbital7.Extensions.Reporting
{
    public abstract class DateReportBase<T> : ReportBase<T>
        where T : IReportBuilder
    {
        public DateTime Date { get; set; }

        public DateReportBase()
            : base()
        {
            this.Date = DateTime.Now.RoundToStartOfDay();
        }

        public override string GetFilename(string extension)
        {
            return this.Name + " " + this.Date.ToFileSystemSafeDateString() + extension;
        }
    }
}
