using System.Collections.Generic;
using System.IO;

namespace System.Web.Mvc
{
    public class ControllerEx : Controller
    {
        public FileContentResult FileInline(byte[] fileContents, string contentType, string fileName)
        {
            this.Response.AppendHeader("Content-Disposition", "inline; filename=" + fileName.Replace(",", ""));
            return File(fileContents, contentType);
        }

        public static SelectList CreateEmptySelectList(string emptyText)
        {
            return new List<Tuple<Guid, string>> { new Tuple<Guid, string>(Guid.Empty, emptyText) }.ToSelectList();
        }

        public SelectList GetEmptySelectList(string emptyText)
        {
            return CreateEmptySelectList(emptyText);
        }

        public ContentResult ClientNavigateTo(string url)
        {
            return Content(GetNavigateJScript(url));
        }

        public static string GetNavigateJScript(string url)
        {
            return "window.location = '" + url + "';";
        }

        public static Stream GetPostedFileStream(HttpPostedFileBase postedFile)
        {
            if (postedFile != null)
                return postedFile.InputStream;
            else
                return null;
        }

        public static byte[] GetPostedFileContents(HttpPostedFileBase postedFile)
        {
            if (postedFile != null)
                return postedFile.InputStream.ReadAll();
            else
                return null;
        }
    }
}
