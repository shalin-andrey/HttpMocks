using System.Collections.Specialized;

namespace HttpMocks.Implementation
{
    public class HttpResponseInfo
    {
        public static HttpResponseInfo Create(int statusCode, byte[] contentBytes, string contentType, NameValueCollection headers)
        {
            return new HttpResponseInfo
            {
                StatusCode = statusCode,
                ContentBytes = contentBytes,
                ContentType = contentType,
                Headers = headers
            };
        }

        public static HttpResponseInfo Create(int statusCode)
        {
            return Create(statusCode, new byte[0], null, new NameValueCollection());
        }

        public int StatusCode { get; set; }
        public NameValueCollection Headers { get; set; }
        public byte[] ContentBytes { get; set; }
        public string ContentType { get; set; }
    }
}