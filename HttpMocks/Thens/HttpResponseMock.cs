using System;
using System.Collections.Specialized;
using System.Threading.Tasks;
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
        public NameValueCollection Headers { get; }
        public int RepeatCount { get; set; }
        public Func<HttpRequestInfo, Task<HttpResponseInfo>> ResponseInfoBuilder { get; set; }
    }
}