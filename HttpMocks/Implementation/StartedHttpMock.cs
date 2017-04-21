using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using HttpMocks.Verifications;

namespace HttpMocks.Implementation
{
    internal class StartedHttpMock : IStartedHttpMock
    {
        private readonly HttpListener httpListener;
        private readonly IHandlingMockQueue handlingMockQueue;
        private readonly List<VerificationResult> verificationMockResults;
        private bool stated;
        private Task listenHttpMockTask;

        public StartedHttpMock(Uri mockUrl, IHandlingMockQueue handlingMockQueue)
        {
            this.handlingMockQueue = handlingMockQueue;
            httpListener = new HttpListener();
            httpListener.Prefixes.Add(mockUrl.ToString());
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
                    context.Response.SendChunked = false;

                    var requestContentBytes = await ReadInputStreamAsync(context.Request.ContentLength64, context.Request.InputStream).ConfigureAwait(false);
                    var httpRequestInfo = HttpRequestInfo.Create(context.Request.HttpMethod,
                        context.Request.Url.LocalPath, context.Request.QueryString,
                        context.Request.Headers, requestContentBytes);
                    var httpResponseInfo = ProcessRequest(httpRequestInfo);
                    LogHttpRequestInfo(httpRequestInfo);
                    var httpResponseInfo = await ProcessRequestAsync(httpRequestInfo).ConfigureAwait(false);
                    LogHttpResponseInfo(httpResponseInfo);
                    await WriteResponseAsync(context, httpResponseInfo).ConfigureAwait(false);
                }
                catch (Exception e)
                {
                    var verificationResult = VerificationResult.Create($"Unhandled exception: {e}");
                    verificationMockResults.Add(verificationResult);
                    context.Response.StatusCode = 500;
                }
                finally
                {
                    context.Response.Close();
                }
            }
        }

        private void LogHttpRequestInfo(HttpRequestInfo httpRequestInfo)
        {
            Console.WriteLine($"HM.Request: {httpRequestInfo.Method} {httpRequestInfo.Path}?{HttpQueryToString(httpRequestInfo.Query)}");
            foreach (var headerName in httpRequestInfo.Headers.AllKeys)
            {
                var headerValue = httpRequestInfo.Headers[headerName];
                Console.WriteLine($"HM.Request: {headerName}: {headerValue}");
            }

            Console.WriteLine("HM.Request:");
            Console.WriteLine($"HM.Request: {httpRequestInfo.ContentBytes.Length}");
        }

        private void LogHttpResponseInfo(HttpResponseInfo httpResponseInfo)
        {
            Console.WriteLine($"HM.Response: HTTP/1.1 {httpResponseInfo.StatusCode}");
            foreach (var headerName in httpResponseInfo.Headers.AllKeys)
            {
                var headerValue = httpResponseInfo.Headers[headerName];
                Console.WriteLine($"HM.Response: {headerName}: {headerValue}");
            }

            Console.WriteLine("HM.Response:");
            Console.WriteLine($"HM.Response: {httpResponseInfo.ContentBytes.Length}");
        }

        private string HttpQueryToString(NameValueCollection query)
        {
            var parameterAndValuePairs = new List<string>();
            foreach (var parameterName in query.AllKeys)
            {
                parameterAndValuePairs.Add($"{parameterName}={query[parameterName]}");
            }
            return string.Join("&", parameterAndValuePairs);
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
                context.Response.ContentLength64 = httpResponseInfo.ContentBytes.Length;
                await
                    context.Response.OutputStream.WriteAsync(httpResponseInfo.ContentBytes, 0, contentBytesLength)
                        .ConfigureAwait(false);
                await context.Response.OutputStream.FlushAsync().ConfigureAwait(false);
            }
        }

        private async Task<HttpResponseInfo> ProcessRequestAsync(HttpRequestInfo httpRequestInfo)
        {
            var handlingInfo = handlingMockQueue.Dequeue(httpRequestInfo.Method, httpRequestInfo.Path);

            if (handlingInfo == null)
            {
                var verificationResult = VerificationResult.Create(
                    $"Actual request {httpRequestInfo.Method} {httpRequestInfo.Path}, but not expected.");
                verificationMockResults.Add(verificationResult);
                return HttpResponseInfo.Create(500);
            }

            if (!handlingInfo.IsUsageCountValid())
            {
                var verificationResult = VerificationResult.Create(
                    $"Actual request {httpRequestInfo.Method} {httpRequestInfo.Path} repeat" +
                    $" count {handlingInfo.UsageCount}, but max expected repeat count {handlingInfo.ResponseMock.RepeatCount}.");
                verificationMockResults.Add(verificationResult);
                return HttpResponseInfo.Create(500);
            }

            var httpResponseMock = handlingInfo.ResponseMock;

            if (httpResponseMock.ResponseInfoBuilder != null)
            {
                return await SafeInvokeResponseInfoBuilderAsync(httpResponseMock.ResponseInfoBuilder, httpRequestInfo).ConfigureAwait(false);
            }

            return HttpResponseInfo.Create(httpResponseMock.StatusCode, httpResponseMock.Content.Bytes,
                httpResponseMock.Content.Type, httpResponseMock.Headers);
        }

        private async Task<HttpResponseInfo> SafeInvokeResponseInfoBuilderAsync(Func<HttpRequestInfo, Task<HttpResponseInfo>> asyncResponseInfoBuilder, HttpRequestInfo httpRequestInfo)
        {
            try
            {
                return await asyncResponseInfoBuilder(httpRequestInfo).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                var verificationResult = VerificationResult.Create($"Unhandled exception from response info builder: {e}");
                verificationMockResults.Add(verificationResult);
                return HttpResponseInfo.Create(500);
            }
        }

        private static async Task<byte[]> ReadInputStreamAsync(long contentLength, Stream stream)
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