using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Orbital7.Extensions.AspNetCore.Mvc;

public class TrimmedFormValueProviderFactory :
    IValueProviderFactory
{
    public Task CreateValueProviderAsync(
        ValueProviderFactoryContext context)
    {
        if (context.ActionContext.HttpContext.Request.HasFormContentType)
        {
            context.ValueProviders.Add(
                new TrimmedFormValueProvider(
                    context.ActionContext.HttpContext.Request.Form));
        }

        return Task.CompletedTask;
    }
}
