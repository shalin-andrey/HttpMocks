using System;
using System.Threading.Tasks;

namespace HttpMocks.Implementation.Core
{
    public interface IHttpListenerWrapper
    {
        void Stop();
        Task<HttpContext> GetContextAsync();

        Uri MockUrl { get; }
    }
}