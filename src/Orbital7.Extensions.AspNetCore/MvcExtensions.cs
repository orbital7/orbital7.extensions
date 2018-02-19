using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;
using Orbital7.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;

namespace Microsoft.AspNetCore.Mvc
{
    public static class MvcExtensions
    {
        public static ModelExplorer GetModelExplorer<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression)
        {
            return ExpressionMetadataProvider.FromLambdaExpression(expression, htmlHelper.ViewData, htmlHelper.MetadataProvider);
        }

        public static bool HasAttribute<TModel, TProperty>(this Expression<Func<TModel, TProperty>> expression, Type attributeType)
        {
            var property = expression.Body as MemberExpression;

            if (property != null)
            {
                var results = property.Member.GetCustomAttributes(attributeType, false);
                return results.Length > 0;
            }

            return false;
        }

        public static void AddOrInsertToExisting(this IDictionary<string, object> attributes, string key, object value, string append = " ")
        {
            if (!attributes.ContainsKey(key))
                attributes.Add(key, value);
            else
                attributes[key] = value + append + attributes[key];
        }

        public static void AddOrAppendToExisting(this IDictionary<string, object> attributes, string key, object value, string append = " ")
        {
            if (!attributes.ContainsKey(key))
                attributes.Add(key, value);
            else
                attributes[key] = attributes[key] + append + value;
        }

        public static void AddIfMissing(this IDictionary<string, object> attributes, string key, object value)
        {
            if (!attributes.ContainsKey(key))
                attributes.Add(key, value);
        }

        public static string GetPropertyDisplayName(this ModelExplorer modelExplorer)
        {
            string displayName = null;
            
            if (modelExplorer.Metadata != null)
                displayName = modelExplorer.Metadata.DisplayName ?? modelExplorer.Metadata.PropertyName;
            
            return displayName;
        }

        public static string GetPropertyDisplayName<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression)
        {
            return htmlHelper.GetModelExplorer(expression).GetPropertyDisplayName();
        }

        public static IHtmlContent EncodedReplace(this IHtmlHelper helper, string input, string pattern, string replacement)
        {
            return new HtmlString(Regex.Replace(helper.Encode(input), pattern, replacement));
        }

        public static IHtmlContent DisplayCurrency(this IHtmlHelper helper, decimal? number, bool addSymbol = false, bool blankIfZero = false)
        {
            if (number.HasValue && (number.Value != 0 || !blankIfZero))
                return new HtmlString("<span style='white-space: nowrap;'>" + number.Value.ToCurrency(addSymbol) + "</span>");
            else
                return new HtmlString(String.Empty);
        }
    }
}
