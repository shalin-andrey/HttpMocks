using System;
using HttpMocks.DebugLoggers;
using HttpMocks.Implementation;
using HttpMocks.Verifications;

namespace HttpMocks
{
    public interface IHttpMockDebugLogger
    {
        void LogRunHttpMock();
        void LogHttpRequest(HttpRequest request);
        void LogHttpResponse(HttpResponse response);
        void LogUnhandledException(Exception exception);
        void LogStopHttpMock(VerificationResult[] verificationResults);
        void LogNotExpected(HttpRequest request);
        void LogCountSpent(HttpRequest request, int usageCount, int repeatCount);
        void LogRequestMatchResult(HttpRequest request, HttpRequestPatternMatchResults matchResults);
    }
}