namespace Orbital7.Extensions.Attributes
{
    public partial class RequiredIfNotAttribute : RequiredIfAttribute//, IClientModelValidator
    {
        public RequiredIfNotAttribute(string dependentProperty, object targetValue)
            : base(dependentProperty, targetValue)
        {
            this.ShouldMatch = false;
        }

        //protected override string ValidationName
        //{
        //    get { return "requiredifnot"; }
        //}
    }
}
