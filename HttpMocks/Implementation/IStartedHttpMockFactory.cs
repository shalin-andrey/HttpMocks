using HttpMocks.Implementation.Core;

namespace HttpMocks.Implementation
{
    internal interface IStartedHttpMockFactory
    {
        IStartedHttpMock Create(IHttpListenerWrapper httpListenerWrapper, IHandlingMockQueue handlingMockQueue);
    }
}