using System;
using HttpMocks.Whens;

namespace HttpMocks.Implementation
{
    public class HttpRequestPattern
    {
        private readonly IHttpRequestMockContentPattern contentPattern;
        private readonly string methodPattern;
        private readonly HttpPathPattern pathPattern;

        public HttpRequestPattern(string methodPattern, string pathPattern, IHttpRequestMockContentPattern contentPattern)
        {
            this.contentPattern = contentPattern;
            this.methodPattern = methodPattern.ToLower();
            this.pathPattern = new HttpPathPattern(pathPattern);
        }

        public bool IsMatch(HttpRequestInfo httpRequestInfo)
        {
            return string.Equals(methodPattern, httpRequestInfo.Method, StringComparison.OrdinalIgnoreCase)
                && pathPattern.IsMatch(httpRequestInfo.Path)
                && contentPattern.IsMatch(httpRequestInfo.ContentBytes, httpRequestInfo.ContentType);
        }
    }
}