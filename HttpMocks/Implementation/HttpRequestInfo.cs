using System.Collections.Specialized;

namespace HttpMocks.Implementation
{
    public class HttpRequestInfo
    {
        public static HttpRequestInfo Create(string method, string path, NameValueCollection query, NameValueCollection headers, byte[] contentBytes)
        {
            return new HttpRequestInfo
            {
                Method = method,
                Headers = headers,
                Query = query,
                Path = path,
                ContentBytes = contentBytes
            };
        }

        public string Method { get; set; }
        public NameValueCollection Headers { get; set; }
        public NameValueCollection Query { get; set; }
        public string Path { get; set; }
        public byte[] ContentBytes { get; set; }
    }
}