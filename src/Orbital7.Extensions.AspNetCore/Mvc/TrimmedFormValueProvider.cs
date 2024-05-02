using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.Globalization;

namespace Microsoft.AspNetCore.Mvc.ModelBinding;

public class TrimmedFormValueProvider
    : FormValueProvider
{
    public TrimmedFormValueProvider(IFormCollection values)
        : base(BindingSource.Form, values, CultureInfo.InvariantCulture)
    { }

    public override ValueProviderResult GetValue(string key)
    {
        ValueProviderResult baseResult = base.GetValue(key);
        string[] trimmedValues = baseResult.Values.Select(v => v?.Trim()).ToArray();
        return new ValueProviderResult(new StringValues(trimmedValues));
    }
}
