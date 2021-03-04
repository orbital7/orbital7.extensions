using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Mvc.ModelBinding
{
    public class TrimmedQueryStringValueProviderFactory
        : IValueProviderFactory
    {
        public Task CreateValueProviderAsync(ValueProviderFactoryContext context)
        {
            context.ValueProviders.Add(new TrimmedQueryStringValueProvider(context.ActionContext.HttpContext.Request.Query));
            return Task.CompletedTask;
        }
    }
}
