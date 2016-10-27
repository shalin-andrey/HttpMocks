using System;
using System.Collections.Generic;

namespace HttpMocks
{
    public class HttpMockPortGenerator
    {
        private readonly HashSet<int> actualPorts;
        private readonly Random random;

        public HttpMockPortGenerator()
        {
            actualPorts = new HashSet<int>();
            random = new Random(DateTime.Now.Millisecond);
        }

        public int GeneratePort()
        {
            int newPort;
            var tryCount = 0;
            do
            {
                newPort = random.Next(20000, 40000);
                tryCount++;
            } while (actualPorts.Contains(newPort) && tryCount < 10);

            if (tryCount >= 10)
            {
                throw new Exception("Generate new port error");
            }

            actualPorts.Add(newPort);
            return newPort;
        }
    }
}