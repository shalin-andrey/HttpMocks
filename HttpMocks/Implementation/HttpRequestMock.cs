using System.Collections.Specialized;
using HttpMocks.Thens;
using HttpMocks.Whens;
using HttpMocks.Whens.HttpRequestMockContentPatterns;

namespace HttpMocks.Implementation
{
    internal class HttpRequestMock
    {
        public HttpRequestMock(string methodPattern, string pathPattern)
        {
            MethodPattern = methodPattern;
            PathPattern = pathPattern;
            Headers = new NameValueCollection();
            ContentPattern = ContentPatterns.Any();
        }

        public string MethodPattern { get; private set; }
        public string PathPattern { get; private set; }
        public NameValueCollection Headers { get; private set; }
        public IHttpRequestMockContentPattern ContentPattern { get; set; }
        public HttpResponseMock Response { get; set; }
    }
}