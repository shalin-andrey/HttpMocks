using System;

namespace HttpMocks.Implementation
{
    internal interface IStartedHttpMockFactory
    {
        IStartedHttpMock Create(Uri mockUrl, IHandlingMockQueue handlingMockQueue);
    }
}