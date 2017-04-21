using System;
using System.Threading.Tasks;
using HttpMocks.Implementation;
using HttpMocks.Thens;
using HttpMocks.Whens.HttpRequestMockContentPatterns;

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

        public IHttpRequestMock Content(byte[] contentBytes, string contentType = null)
        {
            if (contentBytes == null) throw new ArgumentNullException(nameof(contentBytes));

            httpRequestMock.ContentPattern = ContentPatterns.Binary(contentBytes, contentType);
            return this;
        }

        public IHttpRequestMock Content(IHttpRequestMockContentPattern httpRequestMockContentPattern)
        {
            if (httpRequestMockContentPattern == null) throw new ArgumentNullException(nameof(httpRequestMockContentPattern));

            httpRequestMock.ContentPattern = httpRequestMockContentPattern;
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
            return httpResponseMockBuilder = new HttpResponseMockBuilder(httpRequestInfo => Task.FromResult(responseInfoBuilder(httpRequestInfo)));
        }

        public HttpRequestMock Build()
        {
            httpRequestMock.Response = httpResponseMockBuilder.Build();
            return httpRequestMock;
        }

        public ICustomHttpResponseMock ThenResponse(Func<HttpRequestInfo, Task<HttpResponseInfo>> asyncResponseInfoBuilder)
        {
            return httpResponseMockBuilder = new HttpResponseMockBuilder(asyncResponseInfoBuilder);
        }
    }
}