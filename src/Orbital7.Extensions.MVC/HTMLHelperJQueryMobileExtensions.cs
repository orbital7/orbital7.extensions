using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Routing;

namespace System.Web.Mvc.Html
{
    public static class HTMLHelperJQueryMobileExtensions
    {
        public static MvcHtmlString DateEditorFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, bool includeYear, object htmlAttributes = null)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            DateTime? date = (DateTime?)(metadata.Model);

            IDictionary<string, object> attributes = HTMLHelperExtensions.ToAttributesDictionary(htmlAttributes);
            
            if (date != null && date.HasValue)
                attributes.Add("Value", date.Value.ToShortDateString());

            if (includeYear)
                attributes.Add(new KeyValuePair<string, object>("class", "inputDateWithYear"));
            else
                attributes.Add(new KeyValuePair<string, object>("class", "inputDate"));

            return htmlHelper.TextBoxFor(expression, attributes);
        }

        public static MvcHtmlString FlipSwitchFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string textTrue, string textFalse, object htmlAttributes = null)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            bool value = (bool)(metadata.Model ?? false);

            List<SelectListItem> items =
                new List<SelectListItem>()
                    {
                        new SelectListItem() { Text = textFalse, Value = "False", Selected = (!value) },
                        new SelectListItem() { Text = textTrue, Value = "True", Selected = (value) }
                    };

            IDictionary<string, object> attributes = HTMLHelperExtensions.ToAttributesDictionary(htmlAttributes);
            attributes.Add(new KeyValuePair<string, object>("data-role", "slider"));
            attributes.Add(new KeyValuePair<string, object>("data-mini", "true"));
                
            return htmlHelper.DropDownListFor(expression, items, attributes);
        }

        public static MvcHtmlString FlipSwitchYesNoFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null)
        {
            return FlipSwitchFor(htmlHelper, expression, "Yes", "No", htmlAttributes);
        }

        public static MvcHtmlString FlipSwitchOnOffFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null)
        {
            return FlipSwitchFor(htmlHelper, expression, "On", "Off", htmlAttributes);
        }

        public static MvcHtmlString FlipSwitchTrueFalseFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null)
        {
            return FlipSwitchFor(htmlHelper, expression, "True", "False", htmlAttributes);
        }
    }
}
