using System.Linq;
using FluentAssertions;
using HttpMocks.Implementation;
using NUnit.Framework;

namespace HttpMocks.Tests.Integrational
{
    [TestFixture]
    public class UnavailablePortsProviderTest
    {
        [Test]
        public void Test()
        {
            var httpMocks = new HttpMockRepository();
            var httpMock = httpMocks.New("localhost");
            httpMock.Run();

            var unavailablePortsProvider = new UnavailablePortsProvider();
            unavailablePortsProvider.GetUnavailablePorts().Any(port => port == httpMock.MockUri.Port).Should().BeTrue();
        }
    }
}