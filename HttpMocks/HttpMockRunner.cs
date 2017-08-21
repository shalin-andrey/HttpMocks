using HttpMocks.Implementation;
using HttpMocks.Implementation.Core;

namespace HttpMocks
{
    internal class HttpMockRunner : IHttpMockRunner
    {
        private readonly IStartedHttpMockFactory startedHttpMockFactory;
        private readonly IHttpListenerWrapperFactory httpListenerWrapperFactory;

        public HttpMockRunner(IStartedHttpMockFactory startedHttpMockFactory, IHttpListenerWrapperFactory httpListenerWrapperFactory)
        {
            this.startedHttpMockFactory = startedHttpMockFactory;
            this.httpListenerWrapperFactory = httpListenerWrapperFactory;
        }

        public IStartedHttpMock RunMocks(IMockUrlEnumerator mockUrlEnumerator, IHandlingMockQueue handlingMockQueue)
        {
            var httpListenerWrapper = httpListenerWrapperFactory.CreateAndStart(mockUrlEnumerator);
            var startedHttpMock = startedHttpMockFactory.Create(httpListenerWrapper, handlingMockQueue);

            return startedHttpMock;
        }
    }
}