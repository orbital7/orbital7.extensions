using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orbital7.Extensions.Attributes
{
    public partial class RequiredIfNotAttribute
    {
        protected override string ValidationName
        {
            get { return "requiredifnot"; }
        }
    }
}
