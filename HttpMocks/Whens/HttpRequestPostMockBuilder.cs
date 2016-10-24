using System;
using System.Collections.Generic;
using HttpMocks.Thens;
using HttpMocks.Verifications;

namespace HttpMocks
{
    internal class HttpRequestPostMockBuilder : HttpRequestMock, IHttpRequestPostMockBuilder
    {
        public HttpRequestPostMockBuilder(string pathPattern)
            : base("POST", pathPattern)
        {
            headers = new Dictionary<string, string>();

            Content = HttpRequestMockContent.Empty;
        }

        public HttpRequestMockContent Content { get; private set; }

        public IHttpRequestPostMockBuilder WhenHeader(string headerName, string headerValue)
        {
            if (string.IsNullOrEmpty(headerName)) throw new ArgumentNullException(nameof(headerName));

            headers[headerName] = headerValue;
            return this;
        }

        public IHttpRequestPostMockBuilder WhenContent(byte[] contentBytes, string contentType)
        {
            if (contentBytes == null) throw new ArgumentNullException(nameof(contentBytes));

            Content = new HttpRequestMockContent(contentBytes, contentType);

            return this;
        }

        public override VerificationResult[] Verify(byte[] contentBytes, string contentType)
        {
            // todo
            return new VerificationResult[0];
        }
    }
}