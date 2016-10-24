using System.Collections.Specialized;

namespace HttpMocks
{
    internal class HttpRequestMockModel
    {
        public HttpRequestMockModel()
        {
            Headers = new NameValueCollection();
            Content = HttpRequestMockContent.Empty;
        }

        public NameValueCollection Headers { get; private set; }
        public HttpRequestMockContent Content { get; set; }
    }
}