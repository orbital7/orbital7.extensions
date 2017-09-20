using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Orbital7.Extensions.NETFramework.Reporting
{
    public abstract class ReportBase<T> : IReport
        where T : IReportBuilder
    {
        public abstract string Name { get; }

        public abstract Task BuildAsync(T builder);

        public ReportBase()
        {

        }

        public override string ToString()
        {
            return this.Name;
        }

        public virtual string GetFilename(string extension)
        {
            return this.Name + extension;
        }

        public Type GetReportBuilderType()
        {
            return typeof(T);
        }
    }
}
