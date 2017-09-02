using System;
using System.Collections.Generic;
using System.Text;

namespace Orbital7.Extensions
{
    public static class DateTimeHelper
    {
        public static TimeZoneInfo GetTimeZone(string timeZoneId)
        {
            if (!String.IsNullOrEmpty(timeZoneId))
                return TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            else
                return TimeZoneInfo.Utc;
        }
    }
}
