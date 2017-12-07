using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Encodings.Web;

namespace Microsoft.AspNetCore.Mvc
{
    public class TagCloser : IDisposable
    {
        private IHtmlContent ClosingContent { get; set; }

        private string ClosingTags { get; set; }

        private IHtmlHelper HtmlHelper { get; set; }

        private string HtmlPrefixField { get; set; }

        private TagCloser(IHtmlHelper htmlHelper, string htmlPrefixField)
        {
            this.HtmlHelper = htmlHelper;
            if (htmlPrefixField != null)
                this.HtmlPrefixField = htmlPrefixField;
        }

        public TagCloser(IHtmlHelper htmlHelper, string closingTags, string htmlPrefixField = null)
            : this(htmlHelper, htmlPrefixField)
        {
            this.ClosingTags = closingTags;
        }

        public TagCloser(IHtmlHelper htmlHelper, IHtmlContent closingContent, string htmlPrefixField = null)
            : this(htmlHelper, htmlPrefixField)
        {
            this.ClosingContent = closingContent;
        }

        public void Dispose()
        {
            if (this.ClosingContent != null)
                this.ClosingContent.WriteTo(this.HtmlHelper.ViewContext.Writer, HtmlEncoder.Default);
            else
                this.HtmlHelper.ViewContext.Writer.Write(this.ClosingTags);

            if (this.HtmlPrefixField != null)
                this.HtmlHelper.ViewData.TemplateInfo.HtmlFieldPrefix = this.HtmlPrefixField;
        }
    }
}
