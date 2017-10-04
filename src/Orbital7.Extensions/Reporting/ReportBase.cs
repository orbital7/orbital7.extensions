using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbital7.Extensions.Reporting
{
    public abstract class ReportBase<T> : IReport
        where T : IReportBuilder
    {
        public abstract string Key { get; }

        public abstract string Name { get; }

        public abstract ReportNativeType NativeType { get; }

        public ReportScopeType ScopeType { get; set; } = ReportScopeType.All;

        public List<SelectableTuple<string, string>> ScopeSpecificSelectables { get; set; } =
            new List<SelectableTuple<string, string>>();

        public ReportBase()
        {

        }
        
        public abstract Task BuildAsync(T builder);

        public List<string> GatherScopeSpecificSelectedIds()
        {
            return (from x in this.ScopeSpecificSelectables
                    where x.IsSelected && x.CanSelect
                    select x.Item1).Distinct().ToList();
        }

        public List<Guid> GatherScopeSpecificSelectedGuids()
        {
            return (from x in this.ScopeSpecificSelectables
                    where x.IsSelected && x.CanSelect
                    select Guid.Parse(x.Item1)).Distinct().ToList();
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
