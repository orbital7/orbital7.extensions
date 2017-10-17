using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;

namespace Orbital7.Extensions.Reporting
{
    public interface IReport
    {
        string Name { get; }

        string GetFilename(string extension);

        string Key { get; }

        ReportNativeType NativeType { get; }

        ReportScopeType ScopeType { get; set; }

        List<SelectableTuple<string, string>> ScopeSpecificSelectables { get; set; }

        List<string> GatherScopeSpecificSelectedIds();

        Type GetReportBuilderType();
    }
}
