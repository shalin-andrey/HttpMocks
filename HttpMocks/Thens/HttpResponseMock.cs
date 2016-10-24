using System;
using System.Collections.Generic;

namespace HttpMocks
{
    internal class HttpResponseMock : IHttpResponseMock
    {
        private readonly Dictionary<string, string> headers;

        public HttpResponseMock(int statusCode)
        {
            StatusCode = statusCode;
            Content = HttpResponseMockContent.Empty;
        }

        public HttpResponseMockContent Content { get; private set; }
        public int StatusCode { get; }

        public IHttpResponseMock ThenContent(byte[] contentBytes, string contentType)
        {
            Content = new HttpResponseMockContent(contentBytes, contentType);

            return this;
        }

        public IHttpResponseMock ThenHeader(string headerName, string headerValue)
        {
            if (string.IsNullOrEmpty(headerName)) throw new ArgumentNullException(nameof(headerName));

            headers[headerName] = headerValue;

            return this;
        }
    }
}