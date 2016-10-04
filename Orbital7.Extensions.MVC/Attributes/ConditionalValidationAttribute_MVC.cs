using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Orbital7.Extensions.Attributes
{
    public partial class ConditionalValidationAttribute : IClientValidatable
    {
        protected abstract string ValidationName { get; }

        protected virtual IDictionary<string, object> GetExtraValidationParameters()
        {
            return new Dictionary<string, object>();
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule()
            {
                ErrorMessage = FormatErrorMessage(metadata.GetDisplayName()),
                ValidationType = ValidationName,
            };
            string depProp = BuildDependentPropertyId(metadata, context as ViewContext);
            // find the value on the control we depend on; if it's a bool, format it javascript style
            string targetValue = (this.TargetValue ?? "").ToString();
            if (this.TargetValue.GetType() == typeof(bool))
            {
                targetValue = targetValue.ToLower();
            }
            rule.ValidationParameters.Add("dependentproperty", depProp);
            rule.ValidationParameters.Add("targetvalue", targetValue);
            // Add the extra params, if any
            foreach (var param in GetExtraValidationParameters())
            {
                rule.ValidationParameters.Add(param);
            }
            yield return rule;
        }

        private string BuildDependentPropertyId(ModelMetadata metadata, ViewContext viewContext)
        {
            string depProp = viewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(this.DependentProperty);
            // This will have the name of the current field appended to the beginning, because the TemplateInfo's context has had this fieldname appended to it.
            var thisField = metadata.PropertyName + "_";
            if (depProp.StartsWith(thisField))
            {
                depProp = depProp.Substring(thisField.Length);
            }
            return depProp;
        }
    }
}
