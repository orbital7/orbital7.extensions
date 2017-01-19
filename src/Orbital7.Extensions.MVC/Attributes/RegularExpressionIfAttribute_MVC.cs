using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbital7.Extensions.Attributes
{
    public partial class RegularExpressionIfAttribute
    {
        protected override string ValidationName
        {
            get { return "regularexpressionif"; }
        }

        protected override IDictionary<string, object> GetExtraValidationParameters()
        {
            // Set the rule RegEx and the rule param pattern
            return new Dictionary<string, object>
            {
                {"rule", "regex"},
                { "ruleparam", pattern }
            };
        }
    }
}
