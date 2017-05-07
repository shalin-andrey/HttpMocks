using System;
using System.Threading.Tasks;
using HttpMocks.Verifications;

namespace HttpMocks.Implementation
{
    internal interface IStartedHttpMock
    {
        void AppendMocks(HttpRequestMock[] httpRequestMocks);
        Task<VerificationResult[]> StopAsync();

        Uri MockUrl { get; }
    }
}