using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbital7.Extensions.Attributes
{
    public partial class RangeIfAttribute
    {
        protected override string ValidationName
        {
            get { return "rangeif"; }
        }

        protected override IDictionary<string, object> GetExtraValidationParameters()
        {
            // Set the rule Range and the rule param [minumum,maximum]
            return new Dictionary<string, object>
            {
                {"rule", "range"},
                { "ruleparam", string.Format("[{0},{1}]", this.minimum, this.maximum) }
            };
        }
    }
}
