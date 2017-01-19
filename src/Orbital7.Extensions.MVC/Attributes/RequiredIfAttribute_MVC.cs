using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbital7.Extensions.Attributes
{
    public partial class RequiredIfAttribute
    {
        protected override string ValidationName
        {
            get { return "requiredif"; }
        }

        protected override IDictionary<string, object> GetExtraValidationParameters()
        {
            return new Dictionary<string, object>
            {
                { "rule", "required" }
            };
        }
    }
}
