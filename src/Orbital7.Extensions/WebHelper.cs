using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Orbital7.Extensions
{
    // TODO: Remove after move to .NET Standard 2.0 (replaced by Orbital7.Extensions.HttpHelper).
    public static class WebHelper
    {
        public static void UploadFileViaFTP(string ftpServer, string filename, ICredentials credentials)
        {
            var client = GetWebClient(credentials);
            client.UploadFile(ftpServer + new FileInfo(filename).Name, "STOR", filename);
        }

        public static void UploadFileViaFTP(string ftpServer, string filename)
        {
            UploadFileViaFTP(ftpServer, filename);
        }

        public static string DownloadSource(string url, ICredentials credentials)
        {
            var client = GetWebClient(credentials);
            return client.DownloadString(url);
        }

        public static string DownloadSource(string url)
        {
            return DownloadSource(url, null);
        }

        public static byte[] DownloadFileContents(string url, ICredentials credentials)
        {
            var client = GetWebClient(credentials);
            return client.DownloadData(url);
        }

        public static byte[] DownloadFileContents(string url)
        {
            return DownloadFileContents(url, null);
        }

        public static void DownloadFile(string url, string filePath, ICredentials credentials)
        {
            var client = GetWebClient(credentials);
            client.DownloadFile(url, filePath);
        }

        public static void DownloadFile(string url, string filePath)
        {
            DownloadFile(url, filePath, null);
        }

        public static void DownloadFileAsync(string url, string filePath, ICredentials credentials)
        {
            var client = GetWebClient(credentials);
            client.DownloadFileAsync(new Uri(url), filePath);
        }

        public static void DownloadFileAsync(string url, string filePath)
        {
            DownloadFileAsync(url, filePath, null);
        }

        public static bool IsOnlineFile(string filePath)
        {
            return filePath.StartsWith("http://", StringComparison.CurrentCultureIgnoreCase) ||
                     filePath.StartsWith("https://", StringComparison.CurrentCultureIgnoreCase);
        }

        public static bool IsHTMLFile(string filename)
        {
            string filenameLower = filename.ToLower();
            string extension = Path.GetExtension(filenameLower);

            return extension.Equals(".url") || extension.Equals(".htm") || extension.Equals(".html") ||
                filenameLower.Contains(".htm?") || filenameLower.Contains(".html?") ||
                IsASPFile(filename);
        }

        public static bool IsASPFile(string filename)
        {
            string filenameLower = filename.ToLower();
            string extension = Path.GetExtension(filenameLower);

            return extension.Equals(".aspx") || extension.Equals(".asp") ||
                filenameLower.Contains(".aspx?") || filenameLower.Contains(".asp?");
        }

        private static WebClient GetWebClient(ICredentials credentials)
        {
            var webClient = new WebClient();

            if (credentials != null)
                webClient.Credentials = credentials;
            else
                webClient.UseDefaultCredentials = true;

            return webClient;
        }
    }
}
