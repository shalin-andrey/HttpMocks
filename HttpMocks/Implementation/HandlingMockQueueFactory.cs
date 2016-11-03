using System.Collections.Generic;

namespace HttpMocks.Implementation
{
    internal class HandlingMockQueueFactory : IHandlingMockQueueFactory
    {
        public IHandlingMockQueue Create(IEnumerable<HttpRequestMock> httpRequestMocks)
        {
            return new HandlingMockQueue(httpRequestMocks);
        }
    }
}