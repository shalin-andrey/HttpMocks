using System.Collections.Specialized;
using HttpMocks.Thens;

namespace HttpMocks.Implementation
{
    public class HttpRequestMock
    {
        public HttpRequestMock(string method, string pathPattern)
        {
            Method = method;
            PathPattern = pathPattern;
            Headers = new NameValueCollection();
            Content = HttpRequestMockContent.Empty;
        }

        public string Method { get; private set; }
        public string PathPattern { get; private set; }
        public NameValueCollection Headers { get; private set; }
        public HttpRequestMockContent Content { get; set; }
        public HttpResponseMock Response { get; set; }
    }
}