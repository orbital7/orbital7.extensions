using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Orbital7.Extensions.Attributes
{
    public enum GenericCompareOperator
    {
        GreaterThan,
        GreaterThanOrEqual,
        LessThan,
        LessThanOrEqual
    }

    public sealed partial class GenericCompareAttribute : ValidationAttribute//, IClientModelValidator
    {
        private GenericCompareOperator operatorname = GenericCompareOperator.GreaterThanOrEqual;

        public string CompareToPropertyName { get; set; }

        public GenericCompareOperator OperatorName { get { return operatorname; } set { operatorname = value; } }

        public GenericCompareAttribute() : base() { }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string operstring = (OperatorName == GenericCompareOperator.GreaterThan ?
            "greater than " : (OperatorName == GenericCompareOperator.GreaterThanOrEqual ?
            "greater than or equal to " :
            (OperatorName == GenericCompareOperator.LessThan ? "less than " :
            (OperatorName == GenericCompareOperator.LessThanOrEqual ? "less than or equal to " : ""))));
            var basePropertyInfo = validationContext.ObjectType.GetRuntimeProperty(CompareToPropertyName);

            var valOther = (IComparable)basePropertyInfo.GetValue(validationContext.ObjectInstance, null);

            var valThis = (IComparable)value;

            if ((operatorname == GenericCompareOperator.GreaterThan && valThis.CompareTo(valOther) <= 0) ||
                (operatorname == GenericCompareOperator.GreaterThanOrEqual && valThis.CompareTo(valOther) < 0) ||
                (operatorname == GenericCompareOperator.LessThan && valThis.CompareTo(valOther) >= 0) ||
                (operatorname == GenericCompareOperator.LessThanOrEqual && valThis.CompareTo(valOther) > 0))
                return new ValidationResult(base.ErrorMessage);
            return null;
        }

        //public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        //{
        //    string errorMessage = this.FormatErrorMessage(metadata.DisplayName);
        //    ModelClientValidationRule compareRule = new ModelClientValidationRule();
        //    compareRule.ErrorMessage = errorMessage;
        //    compareRule.ValidationType = "genericcompare";
        //    compareRule.ValidationParameters.Add("comparetopropertyname", CompareToPropertyName);
        //    compareRule.ValidationParameters.Add("operatorname", OperatorName.ToString());
        //    yield return compareRule;
        //}
    }
}
