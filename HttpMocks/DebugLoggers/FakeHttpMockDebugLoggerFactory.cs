using System;

namespace HttpMocks.DebugLoggers
{
    internal class FakeHttpMockDebugLoggerFactory : IHttpMockDebugLoggerFactory
    {
        public IHttpMockDebugLogger Create(Uri mockUrl)
        {
            return new FakeHttpMockDebugLogger();
        }
    }
}