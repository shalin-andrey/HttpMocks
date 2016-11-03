using System.Threading.Tasks;
using HttpMocks.Verifications;

namespace HttpMocks.Implementation
{
    internal interface IStartedHttpMock
    {
        void Start();
        Task<VerificationResult[]> StopAsync();
    }
}