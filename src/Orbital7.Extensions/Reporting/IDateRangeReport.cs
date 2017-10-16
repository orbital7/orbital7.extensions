using System;
using System.Collections.Generic;
using System.Text;

namespace Orbital7.Extensions.Reporting
{
    public interface IDateRangeReport
    {
        DateTime StartDate { get; set; }

        DateTime EndDate { get; set; }
    }
}
