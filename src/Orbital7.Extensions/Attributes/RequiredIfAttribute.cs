using System.Collections.Generic;

namespace System.ComponentModel.DataAnnotations
{
    public partial class RequiredIfAttribute : ConditionalValidationAttribute//, IClientModelValidator
    {
        public RequiredIfAttribute(string dependentProperty, object targetValue)
            : base(new TypeSpecificRequiredAttribute(), dependentProperty, targetValue)
        {
            
        }

        //protected override string ValidationName
        //{
        //    get { return "requiredif"; }
        //}

        //protected override IDictionary<string, object> GetExtraValidationParameters()
        //{
        //    return new Dictionary<string, object>
        //    {
        //        { "rule", "required" }
        //    };
        //}
    }
}
