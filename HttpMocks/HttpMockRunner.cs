using System.Collections.Generic;
using System.Linq;
using System.Text;
using HttpMocks.Exceptions;
using HttpMocks.Implementation;
using HttpMocks.Implementation.Core;

namespace HttpMocks
{
    internal class HttpMockRunner : IHttpMockRunner
    {
        private readonly List<IStartedHttpMock> startedHttpMocks;
        private readonly IStartedHttpMockFactory startedHttpMockFactory;
        private readonly IHttpListenerWrapperFactory httpListenerWrapperFactory;

        public HttpMockRunner(IStartedHttpMockFactory startedHttpMockFactory, IHttpListenerWrapperFactory httpListenerWrapperFactory)
        {
            this.startedHttpMockFactory = startedHttpMockFactory;
            this.httpListenerWrapperFactory = httpListenerWrapperFactory;
            startedHttpMocks = new List<IStartedHttpMock>();
        }

        public IStartedHttpMock RunMocks(IMockUrlEnumerator mockUrlEnumerator)
        {
            var httpListenerWrapper = httpListenerWrapperFactory.CreateAndStart(mockUrlEnumerator);
            var startedHttpMock = startedHttpMockFactory.Create(httpListenerWrapper);

            startedHttpMocks.Add(startedHttpMock);

            return startedHttpMock;
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