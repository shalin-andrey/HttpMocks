using System;
using HttpMocks.Implementation;
using HttpMocks.Thens;

namespace HttpMocks.Whens
{
    internal class HttpRequestWithoutContentMockBuilder : IHttpRequestGetMockBuilder, IInternalHttpRequestMockBuilder, IHttpRequestDeleteMockBuilder, IHttpRequestHeadMockBuilder
    {
        private HttpResponseMockBuilder httpResponseMockBuilder;
        private readonly HttpRequestMock httpRequestMock;

        public HttpRequestWithoutContentMockBuilder(string pathPattern)
        {
            httpRequestMock = new HttpRequestMock("GET", pathPattern);
            httpResponseMockBuilder = new HttpResponseMockBuilder(200);
        }

        public IHttpRequestGetMockBuilder WhenHeader(string headerName, string headerValue)
        {
            if (string.IsNullOrEmpty(headerName)) throw new ArgumentNullException(nameof(headerName));

            httpRequestMock.Headers[headerName] = headerValue;
            return this;
        }

        public IHttpResponseMockBuilder ThenResponse(int statusCode)
        {
            return httpResponseMockBuilder = new HttpResponseMockBuilder(statusCode);
        }

        public HttpRequestMock Build()
        {
            httpRequestMock.Response = httpResponseMockBuilder.Build();
            return httpRequestMock;
        }
    }
}