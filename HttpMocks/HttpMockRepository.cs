using System;
using HttpMocks.Exceptions;
using HttpMocks.Implementation;
using HttpMocks.Implementation.Core;

namespace HttpMocks
{
    public class HttpMockRepository
    {
        private static readonly UnavailablePortsProvider unavailablePortsProvider = new UnavailablePortsProvider();
        private static readonly HttpListenerWrapperFactory httpListenerWrapperFactory = new HttpListenerWrapperFactory(new HttpMocksExceptionFactory(unavailablePortsProvider));
        private static readonly StartedHttpMockFactory startedHttpMockFactory = new StartedHttpMockFactory();

        private readonly IHttpMockRunner httpMockRunner;
        private readonly IMockUrlEnumeratorFactory mockUrlEnumeratorFactory;

        public HttpMockRepository()
            : this(new HttpMockRunner(startedHttpMockFactory, httpListenerWrapperFactory), new MockUrlEnumeratorFactory(new HttpMockPortGenerator(unavailablePortsProvider)))
        {
        }

        private HttpMockRepository(IHttpMockRunner httpMockRunner, IMockUrlEnumeratorFactory mockUrlEnumeratorFactory)
        {
            this.httpMockRunner = httpMockRunner;
            this.mockUrlEnumeratorFactory = mockUrlEnumeratorFactory;
        }

        public IHttpMock New(string host, int port = 0)
        {
            if (string.IsNullOrWhiteSpace(host)) throw new ArgumentNullException(nameof(host));
            
            var mockUrlEnumerator = port <= 0
                ? mockUrlEnumeratorFactory.CreateRandomPorts(host)
                : mockUrlEnumeratorFactory.CreateSingle(host, port);
            return New(mockUrlEnumerator);
        }

        public IHttpMock New(Uri url)
        {
            if (url == null) throw new ArgumentNullException(nameof(url));

            var mockUrlEnumerator = mockUrlEnumeratorFactory.CreateSingle(url);
            return New(mockUrlEnumerator);
        }

        public void VerifyAll()
        {
            httpMockRunner.VerifyAll();
        }

        private IHttpMock New(IMockUrlEnumerator mockUrlEnumerator)
        {
            var startedHttpMock = httpMockRunner.RunMocks(mockUrlEnumerator);
            return new HttpMock(startedHttpMock);
        }
    }
}