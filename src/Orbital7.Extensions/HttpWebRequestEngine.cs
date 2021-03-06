﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Orbital7.Extensions
{
    public class HttpWebRequestEngine
    {
        private int RetryAttempts { get; set; }

        private int RetryDelayInMS { get; set; }

        public HttpWebRequestEngine(
            int retryAttempts = 0,
            int retryDelayInMS = 5000)
        {
            this.RetryAttempts = retryAttempts;
            this.RetryDelayInMS = retryDelayInMS;
        }

        public async Task<string> SendRequestAsync(
            string url,
            string contents,
            string method = "POST",
            string authorizationHeader = null,
            string contentType = null,
            List<SerializableTuple<string, string>> headers = null)
        {
            int retryCount = 0;

            while (true)
            {
                try
                {
                    var request = (HttpWebRequest)WebRequest.Create(url);
                    request.Method = method;
                    request.KeepAlive = true;
                    if (!string.IsNullOrEmpty(authorizationHeader))
                        request.Headers["Authorization"] = authorizationHeader;
                    if (!string.IsNullOrEmpty(contentType))
                        request.ContentType = contentType;
                    if (headers != null)
                        foreach (var header in headers)
                            request.Headers[header.Item1] = header.Item2;

                    if (contents.HasText())
                    {
                        var encoding = new UTF8Encoding();
                        byte[] queryBytes = encoding.GetBytes(contents);
                        using (var requestStream = request.GetRequestStream())
                        {

                            requestStream.Write(queryBytes, 0, queryBytes.Length);
                            requestStream.Close();
                        }
                    }

                    try
                    {
                        using (var webResponse = await request.GetResponseAsync())
                        {
                            return await webResponse.ReadAsStringAsync();
                        }
                    }
                    catch (WebException webException)
                    {
                        throw new Exception(await GetWebExeptionMessageAsync(webException));
                    }
                }
                catch (Exception unhandledException)
                {
                    if (retryCount >= this.RetryAttempts)
                        throw unhandledException;
                    else
                        Thread.Sleep(this.RetryDelayInMS);
                }

                retryCount++;
            }
        }

        private async Task<string> GetWebExeptionMessageAsync(
            WebException webException)
        {
            string response = webException.Response != null ?
            await webException.Response.ReadAsStringAsync() : null;

            if (string.IsNullOrWhiteSpace(response))
                response = null;

            return response ?? webException.Message;
        }

        public async Task<string> SendFileUploadRequestAsync(
            string url,
            List<SerializableTuple<string, string>> formParamaters,
            string fileParam,
            string fileName,
            string fileContentType,
            byte[] fileContents,
            string method = "POST",
            string authorizationHeader = null)
        {
            int retryCount = 0;

            while (true)
            {
                try
                {
                    var request = (HttpWebRequest)WebRequest.Create(url);
                    request.Method = method;
                    request.KeepAlive = true;
                    if (!string.IsNullOrEmpty(authorizationHeader))
                        request.Headers["Authorization"] = authorizationHeader;

                    string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
                    byte[] boundaryBytes = Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");
                    request.ContentType = "multipart/form-data; boundary=" + boundary;

                    using (var requestStream = request.GetRequestStream())
                    {
                        var encoding = new UTF8Encoding();

                        foreach (var param in formParamaters)
                        {
                            string formdataTemplate = string.Format(
                                "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}",
                                param.Item1, param.Item2);
                            requestStream.Write(boundaryBytes, 0, boundaryBytes.Length);
                            byte[] formdataBytes = encoding.GetBytes(formdataTemplate);
                            requestStream.Write(formdataBytes, 0, formdataBytes.Length);
                            requestStream.Write(boundaryBytes, 0, boundaryBytes.Length);
                        }

                        var header = string.Format(
                            "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n",
                            fileParam, fileName, fileContentType);
                        byte[] headerBytes = encoding.GetBytes(header);
                        requestStream.Write(headerBytes, 0, headerBytes.Length);
                        requestStream.Write(fileContents, 0, fileContents.Length);

                        byte[] trailer = Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
                        requestStream.Write(trailer, 0, trailer.Length);

                        requestStream.Close();
                    }

                    try
                    {
                        using (var webResponse = await request.GetResponseAsync())
                        {
                            return await webResponse.ReadAsStringAsync();
                        }
                    }
                    catch (WebException webException)
                    {
                        throw new Exception(await GetWebExeptionMessageAsync(webException));
                    }
                }
                catch (Exception unhandledException)
                {
                    if (retryCount >= this.RetryAttempts)
                        throw unhandledException;
                    else
                        Thread.Sleep(this.RetryDelayInMS);
                }

                retryCount++;
            }
        }
    }
}
