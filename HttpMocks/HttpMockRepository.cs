using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        private readonly List<HttpMock> httpMocks;
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
            httpMocks = new List<HttpMock>();
        }

        public IHttpMock New(string host, int port)
        {
            if (string.IsNullOrWhiteSpace(host)) throw new ArgumentNullException(nameof(host));
            if (port <= 0) throw new ArgumentOutOfRangeException(nameof(port));

            var mockUrlEnumerator = mockUrlEnumeratorFactory.CreateSingle(host, port);
            return New(mockUrlEnumerator, 1);
        }

        public IHttpMock New(string host)
        {
            if (string.IsNullOrWhiteSpace(host)) throw new ArgumentNullException(nameof(host));

            var mockUrlEnumerator = mockUrlEnumeratorFactory.CreateRandomPorts(host);
            return New(mockUrlEnumerator, 1);
        }

        public IHttpMock NewCluster(string host, int replicasCount)
        {
            if (string.IsNullOrWhiteSpace(host)) throw new ArgumentNullException(nameof(host));

            var mockUrlEnumerator = mockUrlEnumeratorFactory.CreateRandomPorts(host);
            return New(mockUrlEnumerator, replicasCount);
        }

        public IHttpMock New(Uri url)
        {
            if (url == null) throw new ArgumentNullException(nameof(url));

            var mockUrlEnumerator = mockUrlEnumeratorFactory.CreateSingle(url);
            return New(mockUrlEnumerator, 1);
        }

        public void VerifyAll()
        {
            var httpMockStopTasks = httpMocks.Select(x => x.StopAsync()).ToArray();
            var verificationMockResults = httpMockStopTasks.SelectMany(x => x.Result).ToArray();

            if (verificationMockResults.Length == 0)
            {
                return;
            }

            var resultsString = new StringBuilder();
            foreach (var verificationMockResult in verificationMockResults)
            {
                resultsString.AppendLine(verificationMockResult.Message);
            }
            throw new AssertHttpMockException(resultsString.ToString());
        }

        private IHttpMock New(IMockUrlEnumerator mockUrlEnumerator, int replicasCount)
        {
            var handlingMockQueue = new HandlingMockQueue();
            var startedHttpMocks = Enumerable.Range(0, replicasCount)
                .Select(x => httpMockRunner.RunMocks(mockUrlEnumerator, handlingMockQueue))
                .ToArray();

            var httpMock = new HttpMock(startedHttpMocks, handlingMockQueue);

            httpMocks.Add(httpMock);

            return httpMock;
        }
    }
}