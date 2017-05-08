using System;
using HttpMocks.DebugLoggers;
using HttpMocks.Exceptions;
using HttpMocks.Implementation;
using HttpMocks.Implementation.Core;

namespace HttpMocks
{
    public class HttpMockRepository
    {
        private static readonly IUnavailablePortsProvider unavailablePortsProvider = new UnavailablePortsProvider();
        private static readonly IHttpListenerWrapperFactory httpListenerWrapperFactory = new HttpListenerWrapperFactory(new HttpMocksExceptionFactory(unavailablePortsProvider));
        private static readonly IStartedHttpMockFactory startedHttpMockFactory = new StartedHttpMockFactory();
        private static readonly IHttpMockDebugLoggerFactory httpMockDebugLoggerFactory = new FakeHttpMockDebugLoggerFactory();

        private readonly IHttpMockRunner httpMockRunner;
        private readonly IMockUrlEnumeratorFactory mockUrlEnumeratorFactory;

        public HttpMockRepository()
            : this(new HttpMockRunner(startedHttpMockFactory, httpListenerWrapperFactory, httpMockDebugLoggerFactory), new MockUrlEnumeratorFactory(new HttpMockPortGenerator(unavailablePortsProvider)))
        {
        }

        private HttpMockRepository(IHttpMockRunner httpMockRunner, IMockUrlEnumeratorFactory mockUrlEnumeratorFactory)
        {
            this.httpMockRunner = httpMockRunner;
            this.mockUrlEnumeratorFactory = mockUrlEnumeratorFactory;
        }

        public IHttpMock New(string host, int port)
        {
            if (string.IsNullOrWhiteSpace(host)) throw new ArgumentNullException(nameof(host));
            if (port <= 0) throw new ArgumentOutOfRangeException(nameof(port));

            var mockUrlEnumerator = mockUrlEnumeratorFactory.CreateSingle(host, port);
            return New(mockUrlEnumerator);
        }

        public IHttpMock New(string host)
        {
            if (string.IsNullOrWhiteSpace(host)) throw new ArgumentNullException(nameof(host));

            var mockUrlEnumerator = mockUrlEnumeratorFactory.CreateRandomPorts(host);
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