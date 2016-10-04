using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Orbital7.Extensions.Attributes
{
    public partial class NotEqualAttribute : IClientValidatable
    {
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = ErrorMessage,
                ValidationType = "notequalto",
            };
            rule.ValidationParameters["other"] = OtherProperty;
            yield return rule;
        }
    }
}
