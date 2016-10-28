using System.Collections.Specialized;

namespace HttpMocks.Thens
{
    internal class HttpResponseMock
    {
        public HttpResponseMock(int statusCode)
        {
            StatusCode = statusCode;
            Headers = new NameValueCollection();
            RepeatCount = 1;
        }

        public int StatusCode { get; }
        public HttpResponseMockContent Content { get; set; }
        public NameValueCollection Headers { get; private set; }
        public int RepeatCount { get; set; }
    }
}