using System;
using System.Collections.Specialized;
using HttpMocks.Implementation;

namespace HttpMocks.Thens
{
    internal class HttpResponseMock
    {
        public HttpResponseMock()
        {
            Headers = new NameValueCollection();
            RepeatCount = 1;
        }

        public int StatusCode { get; set; }
        public HttpResponseMockContent Content { get; set; }
        public NameValueCollection Headers { get; private set; }
        public int RepeatCount { get; set; }
        public Func<HttpRequestInfo, HttpResponseInfo> ResponseInfoBuilder { get; set; }
    }
}