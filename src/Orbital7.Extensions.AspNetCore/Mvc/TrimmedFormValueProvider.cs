using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Primitives;
using System.Globalization;

namespace Orbital7.Extensions.AspNetCore.Mvc;

public class TrimmedFormValueProvider :
    FormValueProvider
{
    public TrimmedFormValueProvider(
        IFormCollection values) :
        base(
            BindingSource.Form, 
            values, 
            CultureInfo.InvariantCulture)
    { }

    public override ValueProviderResult GetValue(
        string key)
    {
        var baseResult = base.GetValue(key);
        var trimmedValues = baseResult.Values.Select(v => v?.Trim()).ToArray();
        return new ValueProviderResult(new StringValues(trimmedValues));
    }
}
