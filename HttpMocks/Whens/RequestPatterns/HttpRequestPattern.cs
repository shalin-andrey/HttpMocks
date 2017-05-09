using HttpMocks.DebugLoggers;
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

        public bool IsMatch(HttpRequest request, IHttpMockDebugLogger httpMockDebugLogger)
        {
            var methodIsMatch = requestMock.Method.IsMatch(request.Method);
            var pathIsMatch = requestMock.Path.IsMatch(request.Path);
            var headersIsMatch = requestMock.Headers.IsMatch(request.Headers);
            var queryIsMatch = requestMock.Query.IsMatch(request.Query);
            var contentIsMatch = requestMock.Content.IsMatch(request.ContentBytes, request.ContentType);

            var matchResult = new HttpRequestPatternMatchResults(
                methodIsMatch,
                pathIsMatch,
                queryIsMatch,
                headersIsMatch,
                contentIsMatch);

            httpMockDebugLogger.LogRequestMatchResult(request, matchResult);

            return methodIsMatch
                && pathIsMatch
                && headersIsMatch
                && queryIsMatch
                && contentIsMatch;
        }
    }
}