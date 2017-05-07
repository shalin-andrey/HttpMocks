using System;
using System.Net;
using System.Threading.Tasks;

namespace HttpMocks.Implementation.Core
{
    internal class HttpListenerWrapper : IHttpListenerWrapper
    {
        private readonly HttpListener httpListener;

        public HttpListenerWrapper(Uri mockUrl)
        {
            MockUrl = mockUrl;
            httpListener = new HttpListener();
            httpListener.Prefixes.Add(mockUrl.ToString());
        }

        public void Start()
        {
            if (httpListener.IsListening)
            {
                return;
            }

            httpListener.Start();
        }

        public void Stop()
        {
            if (httpListener.IsListening)
            {
                httpListener.Stop();
            }
        }

        public async Task<HttpContext> GetContextAsync()
        {
            var context = await SafeGetContextAsync().ConfigureAwait(false);
            return context == null 
                ? HttpContext.CreateInvalid()
                : HttpContext.Create(context);
        }

        public Uri MockUrl { get; }

        private async Task<HttpListenerContext> SafeGetContextAsync()
        {
            try
            {
                return await httpListener.GetContextAsync().ConfigureAwait(false);
            }
            catch (ObjectDisposedException)
            {
                return null;
            }
        }
    }
}