namespace Orbital7.Extensions.Data;

public static class ValidationHelper
{
    public static ObjectValidationResult Validate(
        object model)
    {
        return ValidateRecur(model, null);
    }

    public static void EnsureIsValid(
        object model)
    {
        var result = Validate(model);

        if (!result.IsValid)
            throw new ValidationException(result.ToString());
    }

    private static ObjectValidationResult ValidateRecur(
        object model,
        string memberNamePrefix)
    {
        // NOTE: This method currently fails to validate Structs.

        var results = new List<ValidationResult>();
        var context = new ValidationContext(model);

        // Validate the base model.
        var isValid = Validator.TryValidateObject(model, context, results, true);

        // Create the validation result and apply the membername prefix if there
        // is one.
        var validationResult = new ObjectValidationResult()
        {
            IsValid = isValid,
            Results = memberNamePrefix.HasText() ?
                results
                    .Select(x => new ValidationResult(
                        x.ErrorMessage,
                        x.MemberNames.Select(x => $"{memberNamePrefix}.{x}").ToArray()))
                    .ToList() :
                results,
        };

        // Get the properties for the model (exclude any 
        // properties that require parameters, like indexers).
        var properties = model
            .GetType()
            .GetProperties()
            .Where(x => x.GetIndexParameters().Length == 0);

        // Recursively validate any complex properties.
        var stringType = typeof(string);
        var enumerableType = typeof(IEnumerable);
        foreach (var property in properties)
        {
            if (property.PropertyType.IsClass &&
                property.PropertyType != stringType)
            {
                var propertyValue = property.GetValue(model);
                if (propertyValue != null)
                {
                    // If the property is enumerable, we want to recursively loop
                    // through and validate the collection items.
                    if (enumerableType.IsAssignableFrom(property.PropertyType))
                    {
                        int i = 0;
                        foreach (var item in propertyValue as IEnumerable)
                        {
                            var propertyName = $"{property.Name}[{i}]";

                            var itemValidationResult = ValidateRecur(
                                item,
                                memberNamePrefix.HasText() ?
                                    memberNamePrefix + "." + propertyName :
                                    propertyName);

                            validationResult.Append(itemValidationResult);
                        }
                    }
                    // Else recursively validate the property value entity.
                    else
                    {
                        var propertyValidationResult = ValidateRecur(
                            propertyValue,
                            memberNamePrefix.HasText() ?
                                memberNamePrefix + "." + property.Name :
                                property.Name);

                        validationResult.Append(propertyValidationResult);
                    }
                }
            }
        }

        return validationResult;
    }
}
