using System;
using System.Threading.Tasks;

namespace HttpMocks.Implementation.Core
{
    public interface IHttpListenerWrapper
    {
        Uri Prefix { get; }

        void Start();
        void Stop();
        Task<HttpContext> GetContextAsync();
    }
}