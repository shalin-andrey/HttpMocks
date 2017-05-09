using System;
using System.Threading.Tasks;
using HttpMocks.Implementation;
using HttpMocks.Thens;
using HttpMocks.Whens.RequestPatterns;

namespace HttpMocks.Whens
{
    internal class HttpRequestMockBuilder : IHttpRequestMockBuilder
    {
        private HttpResponseMockBuilder httpResponseMockBuilder;
        private readonly HttpRequestMock httpRequestMock;

        internal HttpRequestMockBuilder()
        {
            httpRequestMock = new HttpRequestMock
            {
                Method = MethodPattern.Any(),
                Path = PathPattern.Any(),
                Query = QueryPattern.Any(),
                Content = ContentPattern.Any(),
                Headers = HeadersPattern.Any()
            };
            httpResponseMockBuilder = new HttpResponseMockBuilder(200);
        }

        public IHttpRequestMock Content(IHttpRequestContentPattern httpRequestContentPattern)
        {
            if (httpRequestContentPattern == null) throw new ArgumentNullException(nameof(httpRequestContentPattern));

            httpRequestMock.Content = httpRequestContentPattern;
            return this;
        }

        public IHttpRequestMock Method(IHttpRequestMethodPattern httpRequestMethodPattern)
        {
            if (httpRequestMethodPattern == null) throw new ArgumentNullException(nameof(httpRequestMethodPattern));

            httpRequestMock.Method = httpRequestMethodPattern;
            return this;
        }

        public IHttpRequestMock Headers(IHttpRequestHeadersPattern httpRequestHeadersPattern)
        {
            if (httpRequestHeadersPattern == null) throw new ArgumentNullException(nameof(httpRequestHeadersPattern));

            httpRequestMock.Headers = httpRequestHeadersPattern;
            return this;
        }

        public IHttpRequestMock Path(IHttpRequestPathPattern httpRequestPathPattern)
        {
            if (httpRequestPathPattern == null) throw new ArgumentNullException(nameof(httpRequestPathPattern));

            httpRequestMock.Path = httpRequestPathPattern;
            return this;
        }

        public IHttpRequestMock Query(IHttpRequestQueryPattern httpRequestQueryPattern)
        {
            if (httpRequestQueryPattern == null) throw new ArgumentNullException(nameof(httpRequestQueryPattern));

            httpRequestMock.Query = httpRequestQueryPattern;
            return this;
        }

        public IHttpResponseMock ThenResponse(int statusCode)
        {
            return httpResponseMockBuilder = new HttpResponseMockBuilder(statusCode);
        }

        public IHttpResponseMock ThenResponse()
        {
            return httpResponseMockBuilder = new HttpResponseMockBuilder();
        }

        public ICustomHttpResponseMock ThenResponse(Func<HttpRequest, HttpResponse> responseBuilder)
        {
            return httpResponseMockBuilder = new HttpResponseMockBuilder(httpRequestInfo => Task.FromResult(responseBuilder(httpRequestInfo)));
        }

        public HttpRequestMock Build()
        {
            httpRequestMock.Response = httpResponseMockBuilder.Build();
            return httpRequestMock;
        }

        public ICustomHttpResponseMock ThenResponse(Func<HttpRequest, Task<HttpResponse>> asyncResponseBuilder)
        {
            return httpResponseMockBuilder = new HttpResponseMockBuilder(asyncResponseBuilder);
        }
    }
}