using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using HttpMocks.Thens;
using HttpMocks.Verifications;

namespace HttpMocks
{
    internal class StartedHttpMock
    {
        private readonly HttpListener httpListener;
        private readonly HandlingMockQueue handlingMockQueue;
        private readonly List<VerificationResult> verificationMockResults;
        private bool stated;
        private Task listenHttpMockTask;

        public StartedHttpMock(HttpMock httpMock)
        {
            handlingMockQueue = new HandlingMockQueue(httpMock.Build());
            httpListener = new HttpListener();
            httpListener.Prefixes.Add(httpMock.Prefix);
            verificationMockResults = new List<VerificationResult>();
        }

        public void Start()
        {
            stated = true;
            httpListener.Start();
            listenHttpMockTask = StartAsync();
        }

        public Task<VerificationResult[]> StopAsync()
        {
            stated = false;
            httpListener.Stop();
            return listenHttpMockTask.ContinueWith((task, state) => verificationMockResults.ToArray(), null);
        }

        private async Task StartAsync()
        {
            while (stated)
            {
                var context = await SafeGetContextAsync().ConfigureAwait(false);
                if (context == null)
                {
                    return;
                }

                try
                {
                    var requestContentBytes = await ReadInputStreamToEndAsync(context.Request.ContentLength64, context.Request.InputStream).ConfigureAwait(false);
                    var httpRequestInfo = BuildHttpRequestInfo(context, requestContentBytes);
                    var httpResponseInfo = ProcessRequest(httpRequestInfo);
                    await WriteResponseAsync(context, httpResponseInfo).ConfigureAwait(false);
                }
                catch (Exception e)
                {
                    verificationMockResults.Add(new VerificationResult {Message = $"{e}"});
                }
                finally
                {
                    context.Response.Close();
                }
            }
        }

        private async Task<HttpListenerContext> SafeGetContextAsync()
        {
            try
            {
                return await httpListener.GetContextAsync().ConfigureAwait(false);
            }
            catch (ObjectDisposedException)
            {
                return null;
            }
        }

        private static async Task WriteResponseAsync(HttpListenerContext context, HttpResponseInfo httpResponseInfo)
        {
            context.Response.StatusCode = httpResponseInfo.StatusCode;
            foreach (string headerName in httpResponseInfo.Headers.Keys)
            {
                var headerValue = httpResponseInfo.Headers[headerName];
                context.Response.AddHeader(headerName, headerValue);
            }
            var contentBytesLength = httpResponseInfo.ContentBytes.Length;
            if (contentBytesLength > 0)
            {
                context.Response.ContentType = httpResponseInfo.ContentType;
                await context.Response.OutputStream.WriteAsync(httpResponseInfo.ContentBytes, 0, contentBytesLength).ConfigureAwait(false);
                await context.Response.OutputStream.FlushAsync().ConfigureAwait(false);
            }
        }

        private static HttpRequestInfo BuildHttpRequestInfo(HttpListenerContext context, byte[] requestContentBytes)
        {
            return new HttpRequestInfo
            {
                Method = context.Request.HttpMethod,
                Headers = context.Request.Headers,
                Query = context.Request.QueryString,
                Path = context.Request.Url.LocalPath,
                ContentBytes = requestContentBytes
            };
        }

        private HttpResponseInfo ProcessRequest(HttpRequestInfo httpRequestInfo)
        {
            var httpResponseMock = handlingMockQueue.Dequeue(httpRequestInfo.Method, httpRequestInfo.Path);

            if (httpResponseMock == null)
            {
                verificationMockResults.Add(new VerificationResult{Message = $"Actual request {httpRequestInfo.Method} {httpRequestInfo.Path}, but not expected."});
                return BuildInternalServerErrorResponseInfo();
            }

            return BuildResponseInfo(httpResponseMock);
        }

        private static HttpResponseInfo BuildInternalServerErrorResponseInfo()
        {
            return HttpResponseInfo.Create(500);
        }

        private static HttpResponseInfo BuildResponseInfo(HttpResponseMock httpResponseMock)
        {
            return HttpResponseInfo.Create(httpResponseMock.StatusCode,
                httpResponseMock.Content.Bytes, httpResponseMock.Content.Type, new NameValueCollection());
        }

        private static async Task<byte[]> ReadInputStreamToEndAsync(long contentLength, Stream stream)
        {
            var buffer = new byte[contentLength];
            var offset = 0;
            while (offset < contentLength)
            {
                var currentNeedReadLength = contentLength - offset > buffer.Length
                    ? buffer.Length
                    : contentLength - offset;

                var currentReadedCount = await stream.ReadAsync(buffer, offset, (int)currentNeedReadLength).ConfigureAwait(false);
                offset += currentReadedCount;
            }
            return buffer;
        }
    }
}