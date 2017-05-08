using System;

namespace HttpMocks.DebugLoggers
{
    public class ConsoleHttpMockDebugLoggerFactory : IHttpMockDebugLoggerFactory
    {
        public IHttpMockDebugLogger Create(Uri mockUrl)
        {
            return new ConsoleHttpMockDebugLogger();
        }
    }
}