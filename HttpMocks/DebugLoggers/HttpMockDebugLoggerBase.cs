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

        public virtual void LogHttpRequest(HttpRequestInfo request)
        {
        }

        public virtual void LogHttpResponse(HttpResponseInfo response)
        {
        }

        public virtual void LogUnhandledException(Exception exception)
        {
        }

        public virtual void LogStopHttpMock(VerificationResult[] verificationResults)
        {
        }

        public virtual void LogNotExpected(HttpRequestInfo request)
        {
        }

        public virtual void LogCountSpent(HttpRequestInfo request, int usageCount, int repeatCount)
        {
        }

        public virtual void LogRequestMatchResult(HttpRequestInfo request, HttpRequestPatternMatchResults matchResults)
        {
        }
    }
}