using System;
using HttpMocks.Exceptions;
using HttpMocks.Implementation.Core;

namespace HttpMocks.Implementation
{
    internal class StartedHttpMockFactory : IStartedHttpMockFactory
    {
        private readonly IHttpMocksExceptionFactory httpMocksExceptionFactory;
        private readonly IHttpListenerWrapperFactory httpListenerWrapperFactory;

        public StartedHttpMockFactory(IHttpMocksExceptionFactory httpMocksExceptionFactory, IHttpListenerWrapperFactory httpListenerWrapperFactory)
        {
            this.httpMocksExceptionFactory = httpMocksExceptionFactory;
            this.httpListenerWrapperFactory = httpListenerWrapperFactory;
        }

        public IStartedHttpMock Create(Uri mockUrl, IHandlingMockQueue handlingMockQueue)
        {
            var httpListenerWrapper = httpListenerWrapperFactory.Create(mockUrl);
            return new StartedHttpMock(httpListenerWrapper, handlingMockQueue, httpMocksExceptionFactory);
        }
    }
}