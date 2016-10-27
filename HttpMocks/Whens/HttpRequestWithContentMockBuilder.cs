using System;
using HttpMocks.Implementation;
using HttpMocks.Thens;

namespace HttpMocks.Whens
{
    internal class HttpRequestWithContentMockBuilder : IHttpRequestPostMockBuilder, IInternalHttpRequestMockBuilder
    {
        private readonly HttpRequestMock httpRequestMock;
        private HttpResponseMockBuilder httpResponseMockBuilder;

        public HttpRequestWithContentMockBuilder(string pathPattern)
        {
            httpRequestMock = new HttpRequestMock("POST", pathPattern);
            httpResponseMockBuilder = new HttpResponseMockBuilder(200);
        }

        public IHttpRequestPostMockBuilder WhenHeader(string headerName, string headerValue)
        {
            if (string.IsNullOrEmpty(headerName)) throw new ArgumentNullException(nameof(headerName));

            httpRequestMock.Headers[headerName] = headerValue;

            return this;
        }

        public IHttpRequestPostMockBuilder WhenContent(byte[] contentBytes, string contentType)
        {
            if (contentBytes == null) throw new ArgumentNullException(nameof(contentBytes));

            httpRequestMock.Content = new HttpRequestMockContent(contentBytes, contentType);

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