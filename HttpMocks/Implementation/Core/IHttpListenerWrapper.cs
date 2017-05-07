using System;
using System.Threading.Tasks;

namespace HttpMocks.Implementation.Core
{
    public interface IHttpListenerWrapper
    {
        void Start();
        void Stop();
        Task<HttpContext> GetContextAsync();

        Uri MockUrl { get; }
    }
}