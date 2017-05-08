using System;
using HttpMocks.DebugLoggers;
using HttpMocks.Implementation;
using HttpMocks.Verifications;

namespace HttpMocks
{
    public interface IHttpMockDebugLogger
    {
        void LogRunHttpMock();
        void LogHttpRequest(HttpRequestInfo request);
        void LogHttpResponse(HttpResponseInfo response);
        void LogUnhandledException(Exception exception);
        void LogStopHttpMock(VerificationResult[] verificationResults);
        void LogNotExpected(HttpRequestInfo request);
        void LogCountSpent(HttpRequestInfo request, int usageCount, int repeatCount);
        void LogRequestMatchResult(HttpRequestInfo request, HttpRequestPatternMatchResults matchResults);
    }
}