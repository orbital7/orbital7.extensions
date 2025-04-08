using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Text.RegularExpressions;

namespace Orbital7.Extensions.AspNetCore.Mvc;

public static class MvcExtensions
{
    public static ModelExplorer GetModelExplorer<TModel, TProperty>(
        this IHtmlHelper<TModel> htmlHelper,
        Expression<Func<TModel, TProperty>> expression)
    {
        ModelExpressionProvider modelExpressionProvider = 
            (ModelExpressionProvider)htmlHelper.ViewContext.HttpContext.RequestServices.GetRequiredService(
                typeof(IModelExpressionProvider));
        return modelExpressionProvider.CreateModelExpression(htmlHelper.ViewData, expression).ModelExplorer;
    }
    
    public static void AddOrInsertToExisting(
        this IDictionary<string, object> attributes, 
        string key, 
        object value, 
        string append = " ")
    {
        if (!attributes.ContainsKey(key))
            attributes.Add(key, value);
        else
            attributes[key] = value + append + attributes[key];
    }

    public static void AddOrAppendToExisting(
        this IDictionary<string, object> attributes, 
        string key, 
        object value, 
        string append = " ")
    {
        if (!attributes.ContainsKey(key))
            attributes.Add(key, value);
        else
            attributes[key] = attributes[key] + append + value;
    }

    public static void AddIfMissing(
        this IDictionary<string, object> attributes, 
        string key, 
        object value)
    {
        if (!attributes.ContainsKey(key))
            attributes.Add(key, value);
    }

    public static string? GetPropertyDisplayName(
        this ModelExplorer modelExplorer)
    {
        string? displayName = null;
        
        if (modelExplorer.Metadata != null)
            displayName = modelExplorer.Metadata.DisplayName ?? modelExplorer.Metadata.PropertyName;
        
        return displayName;
    }

    public static string? GetPropertyDisplayName<TModel, TProperty>(
        this IHtmlHelper<TModel> htmlHelper,
        Expression<Func<TModel, TProperty>> expression)
    {
        return htmlHelper
            .GetModelExplorer(expression)
            .GetPropertyDisplayName();
    }

    public static IHtmlContent EncodedReplace(
        this IHtmlHelper helper, 
        string input, 
        string pattern, 
        string replacement)
    {
        return new HtmlString(Regex.Replace(helper.Encode(input), pattern, replacement));
    }

    //public static IHtmlContent DisplayCurrency(
    //    this IHtmlHelper helper,
    //    decimal? number,
    //    bool addSymbol = false,
    //    bool blankIfZero = false)
    //{
    //    if (number.HasValue && (number.Value != 0 || !blankIfZero))
    //        return new HtmlString("<span style='white-space: nowrap;'>" + number.Value.ToCurrencyString(addSymbol) + "</span>");
    //    else
    //        return new HtmlString(string.Empty);
    //}

    public static MvcOptions TrimInputStrings(
        this MvcOptions options)
    {
        int formValueProviderFactoryIndex = options.ValueProviderFactories.IndexOf(
                options.ValueProviderFactories.OfType<FormValueProviderFactory>().Single());
        options.ValueProviderFactories[formValueProviderFactoryIndex] = new TrimmedFormValueProviderFactory();

        int queryStringValueProviderFactoryIndex = options.ValueProviderFactories.IndexOf(
            options.ValueProviderFactories.OfType<QueryStringValueProviderFactory>().Single());
        options.ValueProviderFactories[queryStringValueProviderFactoryIndex] = new TrimmedQueryStringValueProviderFactory();

        return options;
    }
}
