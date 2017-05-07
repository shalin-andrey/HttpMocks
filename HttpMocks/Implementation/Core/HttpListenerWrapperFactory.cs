using System;
using HttpMocks.Exceptions;

namespace HttpMocks.Implementation.Core
{
    internal class HttpListenerWrapperFactory : IHttpListenerWrapperFactory
    {
        private readonly IHttpMocksExceptionFactory httpMocksExceptionFactory;

        public HttpListenerWrapperFactory(IHttpMocksExceptionFactory httpMocksExceptionFactory)
        {
            this.httpMocksExceptionFactory = httpMocksExceptionFactory;
        }

        public IHttpListenerWrapper CreateAndStart(IMockUrlEnumerator mockUrlEnumerator)
        {
            return StartListener(mockUrlEnumerator);
        }

        private IHttpListenerWrapper StartListener(IMockUrlEnumerator mockUrlEnumerator)
        {
            Uri lastMockUrl = null;
            Exception exception = null;

            while (mockUrlEnumerator.MoveNext())
            {
                var currentUrl = mockUrlEnumerator.Current;
                try
                {
                    var httpListenerWrapper = new HttpListenerWrapper(currentUrl);
                    httpListenerWrapper.Start();
                    return httpListenerWrapper;
                }
                catch (Exception e)
                {
                    exception = e;
                }
                lastMockUrl = currentUrl;
            }

            throw httpMocksExceptionFactory.CreateWithDiagnostic(lastMockUrl, "Can't start http listener", exception);
        }
    }
}