using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Orbital7.Extensions
{
    public class HttpHelper
    {
        public static async Task<Stream> DownloadStreamAsync(string url, ICredentials credentials = null)
        {
            return await (await SendRequestAsync(url, credentials)).ReadAsStreamAsync();
        }

        public static async Task<string> DownloadSourceAsync(string url, ICredentials credentials = null)
        {
            return await (await SendRequestAsync(url, credentials)).ReadAsStringAsync();
        }

        public static async Task<byte[]> DownloadFileContentsAsync(string url, ICredentials credentials = null)
        {
            return await (await SendRequestAsync(url, credentials)).ReadAsByteArrayAsync();
        }

        // TODO: Enable after move to .NET Standard 2.0.
        //public static async Task DownloadFileAsync(string url, string filePath, ICredentials credentials = null)
        //{
        //    File.WriteAllBytes(filePath, await DownloadFileContentsAsync(url, credentials));
        //}

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

        private static async Task<HttpContent> SendRequestAsync(string url, ICredentials credentials = null)
        {
            using (var handler = new HttpClientHandler() { Credentials = credentials })
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                return response.Content;
            }
        }
    }
}
