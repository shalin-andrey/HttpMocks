using System;
using System.Linq;
using System.Text;
using HttpMocks.Implementation;

namespace HttpMocks.Exceptions
{
    internal class HttpMocksExceptionFactory : IHttpMocksExceptionFactory
    {
        private readonly IUnavailablePortsProvider unavailablePortsProvider;

        public HttpMocksExceptionFactory(
            IUnavailablePortsProvider unavailablePortsProvider)
        {
            this.unavailablePortsProvider = unavailablePortsProvider;
        }

        public Exception CreateWithDiagnostic(Uri mockUrl, string message, Exception innerException)
        {
            var portIsUanavailable = unavailablePortsProvider
                .GetUnavailablePorts()
                .Any(unavailablePort => unavailablePort == mockUrl.Port);

            var messageBuilder = new StringBuilder();
            messageBuilder.AppendLine($"{message}. MockUrl: {mockUrl}.");
            messageBuilder.AppendLine($"portIsUanavailable: {portIsUanavailable}");

            return new Exception(messageBuilder.ToString(), innerException);
        }
    }
}