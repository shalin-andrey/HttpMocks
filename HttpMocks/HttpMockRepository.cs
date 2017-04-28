using System;
using HttpMocks.Implementation;

namespace HttpMocks
{
    public class HttpMockRepository
    {
        private static readonly HttpMockPortGenerator portGenerator = new HttpMockPortGenerator(new UnavailablePortsProvider());
        private readonly IHttpMockRunner httpMockRunner;

        public HttpMockRepository()
            : this(new HttpMockRunner(new HandlingMockQueueFactory(), new StartedHttpMockFactory()))
        {
        }

        internal HttpMockRepository(IHttpMockRunner httpMockRunner)
        {
            this.httpMockRunner = httpMockRunner;
        }

        public IHttpMock New(string host)
        {
            return New(host, portGenerator.GeneratePort());
        }

        public IHttpMock New(string host, int port)
        {
            if (string.IsNullOrWhiteSpace(host)) throw new ArgumentNullException(nameof(host));
            if (port <= 0) throw new ArgumentNullException(nameof(port));

            var uriBuilder = new UriBuilder("http", host, port);
            return New(uriBuilder.Uri);
        }

        public IHttpMock New(Uri uri)
        {
            return new HttpMock(httpMockRunner, uri);
        }

        public void VerifyAll()
        {
            httpMockRunner.VerifyAll();
        }
    }
}