using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Orbital7.Extensions.Reporting
{
    public enum ReportScopeType
    {
        All,

        Specific,
    }

    public enum ReportNativeType
    {
        Pdf,

        [Display(Name = "Excel Spreadsheet")]
        ExcelSpreadsheet,

        [Display(Name = "Word Document")]
        WordDocument,
    }
}
