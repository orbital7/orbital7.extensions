namespace System.ComponentModel.DataAnnotations;

public partial class RequiredIfNotAttribute : RequiredIfAttribute
{
    public RequiredIfNotAttribute(string dependentProperty, object targetValue)
        : base(dependentProperty, targetValue)
    {
        this.ShouldMatch = false;
    }
}
