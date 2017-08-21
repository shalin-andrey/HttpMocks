using System.Collections.Generic;
using System.Linq;
using HttpMocks.Whens.RequestPatterns;

namespace HttpMocks.Implementation
{
    internal class HandlingMockQueue : IHandlingMockQueue
    {
        private readonly List<HttpRequestMockHandlingInfo> handlingInfos;

        public HandlingMockQueue()
        {
            handlingInfos = new List<HttpRequestMockHandlingInfo>();
        }

        public void Enqueue(HttpRequestMock[] httpRequestMocks)
        {
            lock (handlingInfos)
            {
                handlingInfos.AddRange(GetRequestMockHandlingInfos(httpRequestMocks));
            }
        }

        public HttpRequestMockHandlingInfo Dequeue(HttpRequest httpRequest)
        {
            lock (handlingInfos)
            {
                var handlingInfo = handlingInfos.FirstOrDefault(i => i.RequestPattern.IsMatch(httpRequest) && i.HasAttempts());
                if (handlingInfo == null)
                {
                    handlingInfo = handlingInfos.LastOrDefault(i => i.RequestPattern.IsMatch(httpRequest));
                }

                handlingInfo?.IncreaseUsageCount();
                return handlingInfo;
            }
        }

        public HttpRequestMockHandlingInfo[] GetNotActual()
        {
            lock (handlingInfos)
            {
                return handlingInfos.Where(x => x.HasNotActual()).ToArray();
            }
        }

        private IEnumerable<HttpRequestMockHandlingInfo> GetRequestMockHandlingInfos(IEnumerable<HttpRequestMock> requestMocks)
        {
            return requestMocks
                .Select(r => new HttpRequestMockHandlingInfo(new HttpRequestPattern(r), r))
                .ToArray();
        }
    }
}