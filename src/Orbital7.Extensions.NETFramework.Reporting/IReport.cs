using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Orbital7.Extensions.NETFramework.Reporting
{
    public interface IReport
    {
        string Name { get; }

        string GetFilename(string extension);

        Type GetReportBuilderType();
    }
}
