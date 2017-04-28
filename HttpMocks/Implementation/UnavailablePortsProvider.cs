using System.Linq;
using System.Net.NetworkInformation;

namespace HttpMocks.Implementation
{
    internal class UnavailablePortsProvider : IUnavailablePortsProvider
    {
        public int[] GetUnavailablePorts()
        {
            var ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();

            var activeTcpConnections = ipGlobalProperties.GetActiveTcpConnections();
            var activeTcpListeners = ipGlobalProperties.GetActiveTcpListeners();

            return activeTcpConnections
                .Select(activeTcpConnection => activeTcpConnection.LocalEndPoint.Port)
                .Concat(activeTcpListeners.Select(activeTcpListener => activeTcpListener.Port))
                .Distinct()
                .ToArray();
        }
    }
}