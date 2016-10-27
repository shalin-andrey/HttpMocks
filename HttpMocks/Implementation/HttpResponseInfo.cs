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

        public NameValueCollection Headers { get; private set; }
        public byte[] ContentBytes { get; private set; }
        public int StatusCode { get; private set; }
        public string ContentType { get; private set; }
    }
}