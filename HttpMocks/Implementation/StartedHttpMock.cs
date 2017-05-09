using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HttpMocks.Implementation.Core;
using HttpMocks.Verifications;

namespace HttpMocks.Implementation
{
    internal class StartedHttpMock : IStartedHttpMock
    {
        private readonly IHttpListenerWrapper httpListenerWrapper;
        private readonly IHttpMockDebugLogger httpMockDebugLogger;
        private readonly IHandlingMockQueue handlingMockQueue;
        private readonly List<VerificationResult> verificationMockResults;
        private bool stated;
        private readonly Task listenHttpMockTask;

        public StartedHttpMock(IHttpListenerWrapper httpListenerWrapper, IHttpMockDebugLogger httpMockDebugLogger)
        {
            this.httpListenerWrapper = httpListenerWrapper;
            this.httpMockDebugLogger = httpMockDebugLogger;

            handlingMockQueue = new HandlingMockQueue(httpMockDebugLogger);
            verificationMockResults = new List<VerificationResult>();
            stated = true;
            listenHttpMockTask = StartAsync();
        }

        public async Task<VerificationResult[]> StopAsync()
        {
            stated = false;
            httpListenerWrapper.Stop();
            var verificationResults = await listenHttpMockTask.ContinueWith((task, state) => verificationMockResults.ToArray(), null).ConfigureAwait(false);
            httpMockDebugLogger.LogStopHttpMock(verificationResults);
            return verificationResults;
        }

        public void AppendMocks(HttpRequestMock[] httpRequestMocks)
        {
            handlingMockQueue.Enqueue(httpRequestMocks);
        }

        public Uri MockUrl => httpListenerWrapper.MockUrl;

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
                    httpMockDebugLogger.LogHttpRequest(request);
                    var response = await ProcessRequestAsync(request).ConfigureAwait(false);
                    httpMockDebugLogger.LogHttpResponse(response);
                    await context.WriteResponseAsync(response).ConfigureAwait(false);
                }
                catch (Exception e)
                {
                    var verificationResult = VerificationResult.Create($"Unhandled exception: {e}");
                    verificationMockResults.Add(verificationResult);
                    httpMockDebugLogger.LogUnhandledException(e);
                    await context.WriteResponseAsync(HttpResponse.Create(500)).ConfigureAwait(false);
                }
            }
        }

        private async Task<HttpResponse> ProcessRequestAsync(HttpRequest request)
        {
            var handlingInfo = handlingMockQueue.Dequeue(request);

            if (handlingInfo == null)
            {
                var verificationResult = VerificationResult.Create(
                    $"Actual request {request.Method} {request.Path}, but not expected.");
                verificationMockResults.Add(verificationResult);
                httpMockDebugLogger.LogNotExpected(request);
                return HttpResponse.Create(500);
            }

            if (!handlingInfo.IsUsageCountValid())
            {
                var verificationResult = VerificationResult.Create(
                    $"Actual request {request.Method} {request.Path} repeat" +
                    $" count {handlingInfo.UsageCount}, but max expected repeat count {handlingInfo.ResponseMock.RepeatCount}.");
                verificationMockResults.Add(verificationResult);
                httpMockDebugLogger.LogCountSpent(request, handlingInfo.UsageCount, handlingInfo.ResponseMock.RepeatCount);
                return HttpResponse.Create(500);
            }

            var httpResponseMock = handlingInfo.ResponseMock;

            if (httpResponseMock.ResponseInfoBuilder != null)
            {
                return await SafeInvokeResponseInfoBuilderAsync(httpResponseMock.ResponseInfoBuilder, request).ConfigureAwait(false);
            }

            return HttpResponse.Create(httpResponseMock.StatusCode,
                httpResponseMock.Content.Bytes,
                httpResponseMock.Content.Type,
                httpResponseMock.Headers);
        }

        private async Task<HttpResponse> SafeInvokeResponseInfoBuilderAsync(Func<HttpRequest, Task<HttpResponse>> asyncResponseInfoBuilder, HttpRequest request)
        {
            try
            {
                return await asyncResponseInfoBuilder(request).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                var verificationResult = VerificationResult.Create($"Unhandled exception from response info builder: {e}");
                verificationMockResults.Add(verificationResult);
                return HttpResponse.Create(500);
            }
        }
    }
}