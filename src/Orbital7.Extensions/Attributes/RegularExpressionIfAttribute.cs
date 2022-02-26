using System.Collections.Generic;

namespace System.ComponentModel.DataAnnotations
{
    public partial class RegularExpressionIfAttribute : ConditionalValidationAttribute//, IClientModelValidator
    {
        private readonly string pattern;

        public RegularExpressionIfAttribute(string pattern, string dependentProperty, object targetValue)
            : base(new RegularExpressionAttribute(pattern), dependentProperty, targetValue)
        {
            this.pattern = pattern;
        }

        //protected override string ValidationName
        //{
        //    get { return "regularexpressionif"; }
        //}

        //protected override IDictionary<string, object> GetExtraValidationParameters()
        //{
        //    // Set the rule RegEx and the rule param pattern
        //    return new Dictionary<string, object>
        //    {
        //        {"rule", "regex"},
        //        { "ruleparam", pattern }
        //    };
        //}
    }
}
