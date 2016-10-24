using System.Collections.Generic;
using System.Linq;

namespace HttpMocks
{
    public class HttpMock
    {
        private readonly List<HttpRequestMock> requestMocks;

        public static HttpMock New(string prefix)
        {
            return new HttpMock(prefix);
        }

        private HttpMock(string prefix)
        {
            Prefix = prefix;
            requestMocks = new List<HttpRequestMock>();
        }

        internal string Prefix { get; }
        internal IEnumerable<HttpRequestMock> Requests => requestMocks;

        public IHttpRequestGetMock WhenRequestGet()
        {
            return WhenRequestGet(string.Empty);
        }

        public IHttpRequestGetMock WhenRequestGet(string path)
        {
            var requestMock = new HttpRequestGetMock(path);
            requestMocks.Add(requestMock);
            return requestMock;
        }
    }
}