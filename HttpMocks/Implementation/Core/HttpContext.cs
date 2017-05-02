using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace HttpMocks.Implementation.Core
{
    public class HttpContext
    {
        private readonly HttpListenerContext httpListenerContext;

        public static HttpContext Create(HttpListenerContext httpListenerContext)
        {
            return new HttpContext(httpListenerContext, false);
        }

        public static HttpContext CreateInvalid()
        {
            return new HttpContext(null, false);
        }

        private HttpContext(HttpListenerContext httpListenerContext, bool isInvalid)
        {
            this.httpListenerContext = httpListenerContext;
            IsInvalid = isInvalid;
        }

        public bool IsInvalid { get; }

        public async Task<HttpRequestInfo> ReadRequestAsync()
        {
            var listenerRequest = httpListenerContext.Request;
            var requestContentBytes = await ReadInputStreamAsync(listenerRequest.ContentLength64, listenerRequest.InputStream).ConfigureAwait(false);
            return HttpRequestInfo.Create(listenerRequest.HttpMethod,
                listenerRequest.Url.LocalPath, listenerRequest.QueryString,
                listenerRequest.Headers,
                requestContentBytes,
                listenerRequest.ContentType);
        }

        public async Task WriteResponseAsync(HttpResponseInfo response)
        {
            httpListenerContext.Response.SendChunked = false;
            httpListenerContext.Response.StatusCode = response.StatusCode;
            foreach (string headerName in response.Headers.Keys)
            {
                var headerValue = response.Headers[headerName];
                httpListenerContext.Response.AddHeader(headerName, headerValue);
            }
            var contentBytesLength = response.ContentBytes.Length;
            if (contentBytesLength > 0)
            {
                httpListenerContext.Response.ContentType = response.ContentType;
                httpListenerContext.Response.ContentLength64 = response.ContentBytes.Length;
                await httpListenerContext.Response.OutputStream.WriteAsync(response.ContentBytes, 0, contentBytesLength).ConfigureAwait(false);
                await httpListenerContext.Response.OutputStream.FlushAsync().ConfigureAwait(false);
            }
            httpListenerContext.Response.Close();
        }

        private static async Task<byte[]> ReadInputStreamAsync(long contentLength, Stream stream)
        {
            var buffer = new byte[contentLength];
            var offset = 0;
            while (offset < contentLength)
            {
                var currentNeedReadLength = contentLength - offset > buffer.Length
                    ? buffer.Length
                    : contentLength - offset;

                var currentReadedCount = await stream.ReadAsync(buffer, offset, (int)currentNeedReadLength).ConfigureAwait(false);
                offset += currentReadedCount;
            }
            return buffer;
        }
    }
}