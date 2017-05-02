using System;
using System.Net;
using System.Threading.Tasks;

namespace HttpMocks.Implementation.Core
{
    internal class HttpListenerWrapper : IHttpListenerWrapper
    {
        private readonly HttpListener httpListener;

        public HttpListenerWrapper(Uri prefix)
        {
            Prefix = prefix;
            httpListener = new HttpListener();
            httpListener.Prefixes.Add(prefix.ToString());
        }

        public Uri Prefix { get; }

        public void Start()
        {
            httpListener.Start();
        }

        public void Stop()
        {
            httpListener.Stop();
        }

        public async Task<HttpContext> GetContextAsync()
        {
            var context = await SafeGetContextAsync().ConfigureAwait(false);
            return context == null 
                ? HttpContext.CreateInvalid()
                : HttpContext.Create(context);
        }

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