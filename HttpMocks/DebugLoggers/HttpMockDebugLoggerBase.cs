using System;
using HttpMocks.Implementation;
using HttpMocks.Verifications;

namespace HttpMocks.DebugLoggers
{
    public abstract class HttpMockDebugLoggerBase : IHttpMockDebugLogger
    {
        public virtual void LogRunHttpMock()
        {
        }

        public virtual void LogHttpRequest(HttpRequest request)
        {
        }

        public virtual void LogHttpResponse(HttpResponse response)
        {
        }

        public virtual void LogUnhandledException(Exception exception)
        {
        }

        public virtual void LogStopHttpMock(VerificationResult[] verificationResults)
        {
        }

        public virtual void LogNotExpected(HttpRequest request)
        {
        }

        public virtual void LogCountSpent(HttpRequest request, int usageCount, int repeatCount)
        {
        }

        public virtual void LogRequestMatchResult(HttpRequest request, HttpRequestPatternMatchResults matchResults)
        {
        }
    }
}