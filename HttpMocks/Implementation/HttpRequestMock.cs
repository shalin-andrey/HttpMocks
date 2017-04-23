using HttpMocks.Thens;
using HttpMocks.Whens.RequestPatterns;

namespace HttpMocks.Implementation
{
    internal class HttpRequestMock
    {
        public IHttpRequestMethodPattern Method { get;  set; }
        public IHttpRequestPathPattern Path { get; set; }
        public IHttpRequestHeadersPattern Headers { get; set; }
        public IHttpRequestQueryPattern Query { get; set; }
        public IHttpRequestContentPattern Content { get; set; }
        public HttpResponseMock Response { get; set; }
    }
}