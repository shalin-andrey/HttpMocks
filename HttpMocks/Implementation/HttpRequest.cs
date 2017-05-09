using System.Collections.Specialized;

namespace HttpMocks.Implementation
{
    public class HttpRequest
    {
        public static HttpRequest Create(string method, string path, NameValueCollection query, NameValueCollection headers, byte[] contentBytes, string contentType)
        {
            return new HttpRequest
            {
                Method = method,
                Headers = headers,
                Query = query,
                Path = path,
                ContentBytes = contentBytes,
                ContentType = contentType
            };
        }

        public string Method { get; set; }
        public NameValueCollection Headers { get; set; }
        public NameValueCollection Query { get; set; }
        public string Path { get; set; }
        public byte[] ContentBytes { get; set; }
        public string ContentType { get; set; }
    }
}