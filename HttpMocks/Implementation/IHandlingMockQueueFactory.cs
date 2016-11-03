using System.Collections.Generic;

namespace HttpMocks.Implementation
{
    internal interface IHandlingMockQueueFactory
    {
        IHandlingMockQueue Create(IEnumerable<HttpRequestMock> httpRequestMocks);
    }
}