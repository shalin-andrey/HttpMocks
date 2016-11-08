using System;
using HttpMocks.Implementation;
using HttpMocks.Thens;

namespace HttpMocks.Whens
{
    internal class HttpRequestMockBuilder : IHttpRequestMockBuilder
    {
        private HttpResponseMockBuilder httpResponseMockBuilder;
        private readonly HttpRequestMock httpRequestMock;

        public HttpRequestMockBuilder(string method, string pathPattern)
        {
            httpRequestMock = new HttpRequestMock(method, pathPattern);
            httpResponseMockBuilder = new HttpResponseMockBuilder(200);
        }

        public IHttpRequestMock Content(byte[] contentBytes, string contentType)
        {
            if (contentBytes == null) throw new ArgumentNullException(nameof(contentBytes));

            httpRequestMock.Content = new HttpRequestMockContent(contentBytes, contentType);
            return this;
        }

        public IHttpRequestMock Header(string headerName, string headerValue)
        {
            if (string.IsNullOrEmpty(headerName)) throw new ArgumentNullException(nameof(headerName));

            httpRequestMock.Headers[headerName] = headerValue;
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

        public ICustomHttpResponseMock ThenResponse(Func<HttpRequestInfo, HttpResponseInfo> responseInfoBuilder)
        {
            return httpResponseMockBuilder = new HttpResponseMockBuilder(responseInfoBuilder);
        }

        public HttpRequestMock Build()
        {
            httpRequestMock.Response = httpResponseMockBuilder.Build();
            return httpRequestMock;
        }
    }
}