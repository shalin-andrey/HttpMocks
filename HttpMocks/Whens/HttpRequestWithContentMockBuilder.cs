using System;
using HttpMocks.Thens;

namespace HttpMocks.Whens
{
    internal class HttpRequestWithContentMockBuilder : IHttpRequestPostMockBuilder, IInternalHttpRequestMockBuilder
    {
        private readonly HttpRequestMock httpRequestMock;

        public HttpRequestWithContentMockBuilder(string pathPattern)
        {
            httpRequestMock = new HttpRequestMock("POST", pathPattern);
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
            return new HttpResponseMockBuilder(statusCode);
        }

        public HttpRequestMock Build()
        {
            return httpRequestMock;
        }
    }
}