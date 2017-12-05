using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Web.Routing;

namespace System.Web.Mvc.Html
{
    public static class HTMLHelperExtensions
    {
        public static MvcHtmlString EncodedReplace(this HtmlHelper helper, string input, string pattern, string replacement)
        {
            return new MvcHtmlString(Regex.Replace(helper.Encode(input), pattern, replacement));
        }

        public static MvcHtmlString DisplayCurrency(this HtmlHelper helper, double? number, bool addSymbol = false, bool blankIfZero = false)
        {
            if (number.HasValue && (number.Value != 0 || !blankIfZero))
                return new MvcHtmlString("<span style='white-space: nowrap;'>" + number.Value.ToCurrency(addSymbol) + "</span>");
            else
                return new MvcHtmlString(String.Empty);
        }

        public static MvcHtmlString NumberEditorFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);

            IDictionary<string, object> attributes = ToAttributesDictionary(htmlAttributes);
            RangeAttribute rangeAttribute = metadata.ContainerType.GetPropertyAttribute<RangeAttribute>(metadata.PropertyName);
            if (rangeAttribute != null)
            {
                attributes.Add("type", "number");
                attributes.Add("min", rangeAttribute.Minimum);
                attributes.Add("max", rangeAttribute.Maximum);
            }

            return htmlHelper.TextBoxFor(expression, attributes);
        }

        public static MvcHtmlString EnumSortedDropDownListFor<TModel, TEnum>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression, object htmlAttributes = null)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            var selectList = EnumHelper.GetSelectList(metadata.ModelType).OrderBy(i => i.Text).ToList();

            return htmlHelper.DropDownListFor(expression, selectList, ToAttributesDictionary(htmlAttributes));
        }

        public static MvcHtmlString NullableEnumSortedDropDownListFor<TModel, TEnum>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression, string nullText, object htmlAttributes = null)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            var selectList = EnumHelper.GetSelectList(metadata.ModelType).OrderBy(i => i.Text).ToList();
            selectList.First().Text = nullText;

            return htmlHelper.DropDownListFor(expression, selectList, ToAttributesDictionary(htmlAttributes));
        }

        internal static IDictionary<string, object> ToAttributesDictionary(object htmlAttributes)
        {
            IDictionary<string, object> attributes = null;

            if (htmlAttributes == null)
                attributes = new RouteValueDictionary();
            else
                attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);

            return attributes;
        }
    }
}
