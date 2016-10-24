using System;
using HttpMocks.Thens;

namespace HttpMocks.Whens
{
    internal class HttpRequestWihtoutContentMockBuilder : IHttpRequestGetMockBuilder, IInternalHttpRequestMockBuilder
    {
        private HttpResponseMockBuilder httpResponseMockBuilder;
        private readonly HttpRequestMock httpRequestMock;

        public HttpRequestWihtoutContentMockBuilder(string pathPattern)
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
            return httpRequestMock;
        }
    }
}