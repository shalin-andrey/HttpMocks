using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HttpMocks.Exceptions;
using HttpMocks.Implementation;
using HttpMocks.Verifications;

namespace HttpMocks
{
    internal class HttpMockRunner : IHttpMockRunner
    {
        private readonly List<IStartedHttpMock> startedHttpMocks;
        private readonly IHandlingMockQueueFactory handlingMockQueueFactory;
        private readonly IStartedHttpMockFactory startedHttpMockFactory;

        public HttpMockRunner(IHandlingMockQueueFactory handlingMockQueueFactory, IStartedHttpMockFactory startedHttpMockFactory)
        {
            this.handlingMockQueueFactory = handlingMockQueueFactory;
            this.startedHttpMockFactory = startedHttpMockFactory;
            startedHttpMocks = new List<IStartedHttpMock>();
        }

        public void RunMocks(Uri mockUrl, HttpRequestMock[] httpRequestMocks)
        {
            var handlingMockQueue = handlingMockQueueFactory.Create(httpRequestMocks);
            var startedHttpMock = startedHttpMockFactory.Create(mockUrl, handlingMockQueue);
            startedHttpMock.Start();

            startedHttpMocks.Add(startedHttpMock);
        }

        public void VerifyAll()
        {
            var stopMockTasks = startedHttpMocks
                .Select(startedHttpMock => startedHttpMock.StopAsync())
                .ToArray();

            var verificationMockResults = stopMockTasks.SelectMany(t => t.Result).ToArray();

            if (verificationMockResults.Length == 0)
            {
                return;
            }
            
            var resultsString = new StringBuilder();
            foreach (var verificationMockResult in verificationMockResults)
            {
                resultsString.AppendLine(verificationMockResult.Message);
            }
            throw new AssertHttpMockException(resultsString.ToString());
        }
    }
}