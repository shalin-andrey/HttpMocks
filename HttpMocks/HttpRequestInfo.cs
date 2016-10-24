using System.Collections.Specialized;

namespace HttpMocks
{
    public class HttpRequestInfo
    {
        public string Method { get; set; }
        public NameValueCollection Headers { get; set; }
        public NameValueCollection Query { get; set; }
        public string Path { get; set; }
        public byte[] ContentBytes { get; set; }
    }
}