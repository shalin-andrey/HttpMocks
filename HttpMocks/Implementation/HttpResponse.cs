using System.Collections.Specialized;

namespace HttpMocks.Implementation
{
    public class HttpResponse
    {
        public static HttpResponse Create(int statusCode, byte[] contentBytes, string contentType, NameValueCollection headers)
        {
            return new HttpResponse
            {
                StatusCode = statusCode,
                ContentBytes = contentBytes,
                ContentType = contentType,
                Headers = headers
            };
        }

        public static HttpResponse Create(int statusCode)
        {
            return Create(statusCode, new byte[0], null, new NameValueCollection());
        }

        public int StatusCode { get; set; }
        public NameValueCollection Headers { get; set; }
        public byte[] ContentBytes { get; set; }
        public string ContentType { get; set; }
    }
}