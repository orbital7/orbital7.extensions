using System;
using System.Collections.Generic;
using System.Text;

namespace Orbital7.Extensions.NETFramework.Reporting
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
    }
}
