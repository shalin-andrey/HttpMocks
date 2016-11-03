using System;

namespace HttpMocks.Implementation
{
    internal class StartedHttpMockFactory : IStartedHttpMockFactory
    {
        public IStartedHttpMock Create(Uri mockUrl, IHandlingMockQueue handlingMockQueue)
        {
            return new StartedHttpMock(mockUrl, handlingMockQueue);
        }
    }
}