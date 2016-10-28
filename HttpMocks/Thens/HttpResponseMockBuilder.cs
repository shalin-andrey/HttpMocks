using System;

namespace HttpMocks.Thens
{
    internal class HttpResponseMockBuilder : IHttpResponseMockBuilder, IInternalHttpResponseMockBuilder
    {
        private readonly HttpResponseMock httpResponseMock;

        public HttpResponseMockBuilder(int statusCode)
        {
            httpResponseMock = new HttpResponseMock(statusCode);
            httpResponseMock.Content = HttpResponseMockContent.Empty;
        }

        public IHttpResponseMockBuilder ThenContent(byte[] contentBytes, string contentType)
        {
            httpResponseMock.Content = new HttpResponseMockContent(contentBytes, contentType);

            return this;
        }

        public IHttpResponseMockBuilder ThenHeader(string headerName, string headerValue)
        {
            if (string.IsNullOrEmpty(headerName)) throw new ArgumentNullException(nameof(headerName));

            httpResponseMock.Headers[headerName] = headerValue;

            return this;
        }

        public IHttpResponseMockBuilder Repeat(int count)
        {
            httpResponseMock.RepeatCount = count;

            return this;
        }

        public IHttpResponseMockBuilder RepeatAny()
        {
            httpResponseMock.RepeatCount = int.MaxValue;

            return this;
        }

        public HttpResponseMock Build()
        {
            return httpResponseMock;
        }
    }
}