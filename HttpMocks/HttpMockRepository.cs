using System;

namespace HttpMocks
{
    public class HttpMockRepository
    {
        private static readonly HttpMockPortGenerator portGenerator = new HttpMockPortGenerator();
        private static readonly HttpMockRunner httpMockRunner = new HttpMockRunner();

        public HttpMock New(string host)
        {
            return New(host, portGenerator.GeneratePort());
        }

        public HttpMock New(string host, int port)
        {
            if (string.IsNullOrWhiteSpace(host)) throw new ArgumentNullException(nameof(host));
            if (port <= 0) throw new ArgumentNullException(nameof(port));

            var uriBuilder = new UriBuilder("http", host, port);
            return New(uriBuilder.Uri);
        }

        public HttpMock New(Uri uri)
        {
            return new HttpMock(httpMockRunner, uri);
        }

        public void VerifyAll()
        {
            httpMockRunner.VerifyAll();
        }
    }
}