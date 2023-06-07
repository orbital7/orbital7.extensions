namespace System.ComponentModel.DataAnnotations;

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
