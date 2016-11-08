using System.Collections.Specialized;

namespace HttpMocks.Implementation
{
    public class HttpResponseInfoBuilder
    {
        private readonly NameValueCollection headers;
        private byte[] contentBytes;
        private string contentType;
        private int statusCode;

        public HttpResponseInfoBuilder()
        {
            headers = new NameValueCollection();
            contentBytes = new byte[0];
            contentType = string.Empty;
            statusCode = 200;
        }

        public HttpResponseInfoBuilder SetHeader(string headerName, string headerValue)
        {
            headers[headerName] = headerValue;
            return this;
        }

        public HttpResponseInfoBuilder SetHeaders(NameValueCollection newHeaders)
        {
            foreach (string newHeaderName in newHeaders.Keys)
            {
                headers[newHeaderName] = newHeaders[newHeaderName];
            }
            return this;
        }

        public HttpResponseInfoBuilder SetContent(byte[] newContentBytes, string newContentType)
        {
            contentBytes = newContentBytes;
            contentType = newContentType;
            return this;
        }

        public HttpResponseInfoBuilder SetStatusCode(int newStatusCode)
        {
            statusCode = newStatusCode;
            return this;
        }

        public HttpResponseInfo Build()
        {
            return HttpResponseInfo.Create(statusCode, contentBytes, contentType, headers);
        }
    }
}