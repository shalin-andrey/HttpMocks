using System.Collections.Generic;
using System.Linq;
using HttpMocks.Whens.RequestPatterns;

namespace HttpMocks.Implementation
{
    internal class HandlingMockQueue : IHandlingMockQueue
    {
        private readonly IHttpMockDebugLogger httpMockDebugLogger;
        private readonly List<HttpRequestMockHandlingInfo> handlingInfos;

        public HandlingMockQueue(IHttpMockDebugLogger httpMockDebugLogger)
        {
            this.httpMockDebugLogger = httpMockDebugLogger;
            handlingInfos = new List<HttpRequestMockHandlingInfo>();
        }

        public void Enqueue(HttpRequestMock[] httpRequestMocks)
        {
            lock (handlingInfos)
            {
                handlingInfos.AddRange(GetRequestMockHandlingInfos(httpRequestMocks));
            }
        }

        public HttpRequestMockHandlingInfo Dequeue(HttpRequestInfo httpRequestInfo)
        {
            lock (handlingInfos)
            {
                var handlingInfo = handlingInfos.FirstOrDefault(i => i.RequestPattern.IsMatch(httpRequestInfo, httpMockDebugLogger) && i.HasAttempts());
                if (handlingInfo == null)
                {
                    handlingInfo = handlingInfos.LastOrDefault(i => i.RequestPattern.IsMatch(httpRequestInfo, httpMockDebugLogger));
                }

                handlingInfo?.IncreaseUsageCount();
                return handlingInfo;
            }
        }

        private IEnumerable<HttpRequestMockHandlingInfo> GetRequestMockHandlingInfos(IEnumerable<HttpRequestMock> requestMocks)
        {
            return requestMocks
                .Select(r => new HttpRequestMockHandlingInfo(new HttpRequestPattern(r), r.Response))
                .ToArray();
        }
    }
}