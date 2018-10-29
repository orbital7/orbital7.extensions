using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Orbital7.Extensions.Reporting
{
    public abstract class DateRangeReportBase<T> : ReportBase<T>, IDateRangeReport
        where T : IReportBuilder
    {
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        public DateRangeReportBase()
            : base()
        {
            this.StartDate = DateTime.Now.RoundToStartOfMonth();
            this.EndDate = DateTime.Now.RoundToEndOfMonth();
        }

        public override string GetFilename(string extension)
        {
            if (this.StartDate == this.EndDate)
                return this.Name + " " +
                    this.StartDate.FormatAsFileSystemSafeDate() + extension;
            else
                return this.Name + " " + 
                    this.StartDate.FormatAsFileSystemSafeDate() + " to " + 
                    this.EndDate.FormatAsFileSystemSafeDate() + extension;
        }
    }
}
