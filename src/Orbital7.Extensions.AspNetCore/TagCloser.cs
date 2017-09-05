using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Microsoft.AspNetCore.Mvc
{
    public class TagCloser : IDisposable
    {
        private string ClosingTags { get; set; }

        private TextWriter TextWriter { get; set; }

        public TagCloser(IHtmlHelper htmlHelper, string closingTags)
        {
            this.TextWriter = htmlHelper.ViewContext.Writer;
            this.ClosingTags = closingTags;
        }

        public void Dispose()
        {
            this.TextWriter.Write(this.ClosingTags);
        }
    }
}
