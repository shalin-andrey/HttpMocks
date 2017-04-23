using HttpMocks.Implementation;

namespace HttpMocks.Whens.RequestPatterns
{
    internal class HttpRequestPattern
    {
        private readonly HttpRequestMock requestMock;
        
        public HttpRequestPattern(HttpRequestMock requestMock)
        {
            this.requestMock = requestMock;
        }

        public bool IsMatch(HttpRequestInfo httpRequestInfo)
        {
            return requestMock.Method.IsMatch(httpRequestInfo.Method)
                && requestMock.Path.IsMatch(httpRequestInfo.Path)
                && requestMock.Headers.IsMatch(httpRequestInfo.Headers)
                && requestMock.Query.IsMatch(httpRequestInfo.Query)
                && requestMock.Content.IsMatch(httpRequestInfo.ContentBytes, httpRequestInfo.ContentType);
        }
    }
}