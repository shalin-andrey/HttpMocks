using System;
using System.Collections.Generic;
using HttpMocks.Verifications;

namespace HttpMocks
{
    internal class HttpRequestGetMock : HttpRequestMock, IHttpRequestGetMock
    {
        private readonly Dictionary<string, string> headers;

        public HttpRequestGetMock(string pathPattern)
            : base("GET", pathPattern)
        {
            headers = new Dictionary<string, string>();
        }

        public IHttpRequestGetMock WhenHeader(string headerName, string headerValue)
        {
            if (string.IsNullOrEmpty(headerName)) throw new ArgumentNullException(nameof(headerName));

            headers[headerName] = headerValue;
            return this;
        }

        public override VerificationResult[] Verify(byte[] contentBytes, string contentType)
        {
            return new VerificationResult[0];
        }
    }
}