using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;
using HttpMocks.Exceptions;
using HttpMocks.Implementation.Core;
using HttpMocks.Verifications;

namespace HttpMocks.Implementation
{
    internal class StartedHttpMock : IStartedHttpMock
    {
        private readonly IHttpListenerWrapper httpListenerWrapper;
        private readonly IHandlingMockQueue handlingMockQueue;
        private readonly IHttpMocksExceptionFactory httpMocksExceptionFactory;
        private readonly List<VerificationResult> verificationMockResults;
        private bool stated;
        private Task listenHttpMockTask;

        public StartedHttpMock(IHttpListenerWrapper httpListenerWrapper, IHandlingMockQueue handlingMockQueue, IHttpMocksExceptionFactory httpMocksExceptionFactory)
        {
            this.httpListenerWrapper = httpListenerWrapper;
            this.handlingMockQueue = handlingMockQueue;
            this.httpMocksExceptionFactory = httpMocksExceptionFactory;
            verificationMockResults = new List<VerificationResult>();
        }

        public void Start()
        {
            stated = true;
            try
            {
                httpListenerWrapper.Start();
            }
            catch (Exception exception)
            {
                throw httpMocksExceptionFactory.CreateWithDiagnostic(httpListenerWrapper.Prefix, "Can't start http listener", exception);
            }
            listenHttpMockTask = StartAsync();
        }

        public Task<VerificationResult[]> StopAsync()
        {
            stated = false;
            httpListenerWrapper.Stop();
            return listenHttpMockTask.ContinueWith((task, state) => verificationMockResults.ToArray(), null);
        }

        private async Task StartAsync()
        {
            while (stated)
            {
                var context = await httpListenerWrapper.GetContextAsync().ConfigureAwait(false);
                if (context.IsInvalid)
                {
                    return;
                }

                try
                {
                    var request = await context.ReadRequestAsync().ConfigureAwait(false);

                    LogHttpRequestInfo(request);

                    var response = await ProcessRequestAsync(request).ConfigureAwait(false);

                    LogHttpResponseInfo(response);

                    await context.WriteResponseAsync(response).ConfigureAwait(false);
                }
                catch (Exception e)
                {
                    var verificationResult = VerificationResult.Create($"Unhandled exception: {e}");
                    verificationMockResults.Add(verificationResult);
                    await context.WriteResponseAsync(HttpResponseInfo.Create(500)).ConfigureAwait(false);
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

        private async Task<HttpResponseInfo> ProcessRequestAsync(HttpRequestInfo request)
        {
            var handlingInfo = handlingMockQueue.Dequeue(request);

            if (handlingInfo == null)
            {
                var verificationResult = VerificationResult.Create(
                    $"Actual request {request.Method} {request.Path}, but not expected.");
                verificationMockResults.Add(verificationResult);
                return HttpResponseInfo.Create(500);
            }

            if (!handlingInfo.IsUsageCountValid())
            {
                var verificationResult = VerificationResult.Create(
                    $"Actual request {request.Method} {request.Path} repeat" +
                    $" count {handlingInfo.UsageCount}, but max expected repeat count {handlingInfo.ResponseMock.RepeatCount}.");
                verificationMockResults.Add(verificationResult);
                return HttpResponseInfo.Create(500);
            }

            var httpResponseMock = handlingInfo.ResponseMock;

            if (httpResponseMock.ResponseInfoBuilder != null)
            {
                return await SafeInvokeResponseInfoBuilderAsync(httpResponseMock.ResponseInfoBuilder, request).ConfigureAwait(false);
            }

            return HttpResponseInfo.Create(httpResponseMock.StatusCode,
                httpResponseMock.Content.Bytes,
                httpResponseMock.Content.Type,
                httpResponseMock.Headers);
        }

        private async Task<HttpResponseInfo> SafeInvokeResponseInfoBuilderAsync(Func<HttpRequestInfo, Task<HttpResponseInfo>> asyncResponseInfoBuilder, HttpRequestInfo request)
        {
            try
            {
                return await asyncResponseInfoBuilder(request).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                var verificationResult = VerificationResult.Create($"Unhandled exception from response info builder: {e}");
                verificationMockResults.Add(verificationResult);
                return HttpResponseInfo.Create(500);
            }
        }
    }
}