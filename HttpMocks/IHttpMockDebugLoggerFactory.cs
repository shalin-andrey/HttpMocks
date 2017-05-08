using System;

namespace HttpMocks
{
    public interface IHttpMockDebugLoggerFactory
    {
        IHttpMockDebugLogger Create(Uri mockUrl);
    }
}