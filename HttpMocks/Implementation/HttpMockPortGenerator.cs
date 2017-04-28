using System;
using System.Collections.Generic;

namespace HttpMocks.Implementation
{
    internal class HttpMockPortGenerator
    {
        private readonly int minPortValue;
        private readonly int maxPortValue;
        private const int defaultMinPortValue = 20000;
        private const int defaultMaxPortValue = 60000;
        private const int maxTryCount = 100;
        private readonly HashSet<int> actualPorts;
        private readonly Random random;

        internal HttpMockPortGenerator(IUnavailablePortsProvider unavailablePortsProvider, int minPortValue = defaultMinPortValue, int maxPortValue = defaultMaxPortValue)
        {
            this.minPortValue = minPortValue;
            this.maxPortValue = maxPortValue;
            actualPorts = new HashSet<int>(unavailablePortsProvider.GetUnavailablePorts());
            random = new Random(DateTime.Now.Millisecond);
        }

        public int GeneratePort()
        {
            int newPort;
            var tryCount = 0;
            do
            {
                newPort = random.Next(minPortValue, maxPortValue);
                tryCount++;
            } while (actualPorts.Contains(newPort) && tryCount < maxTryCount);

            if (tryCount >= maxTryCount)
            {
                throw new Exception($"Generate new port '{newPort}' error");
            }

            actualPorts.Add(newPort);
            return newPort;
        }
    }
}