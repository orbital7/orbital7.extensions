namespace Orbital7.Extensions.Data;

public partial class RequiredIfNotAttribute : RequiredIfAttribute
{
    public RequiredIfNotAttribute(string dependentProperty, object targetValue)
        : base(dependentProperty, targetValue)
    {
        ShouldMatch = false;
    }
}
