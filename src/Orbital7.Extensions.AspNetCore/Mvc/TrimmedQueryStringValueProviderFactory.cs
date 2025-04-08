using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Orbital7.Extensions.AspNetCore.Mvc;

public class TrimmedQueryStringValueProviderFactory :
    IValueProviderFactory
{
    public Task CreateValueProviderAsync(
        ValueProviderFactoryContext context)
    {
        context.ValueProviders.Add(
            new TrimmedQueryStringValueProvider(
                context.ActionContext.HttpContext.Request.Query));

        return Task.CompletedTask;
    }
}
