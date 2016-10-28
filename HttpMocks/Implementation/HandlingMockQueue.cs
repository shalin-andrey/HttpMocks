using System.Collections.Generic;
using System.Linq;

namespace HttpMocks.Implementation
{
    internal class HandlingMockQueue
    {
        private readonly List<HttpRequestMockHandlingInfo> handlingInfos;

        public HandlingMockQueue(IEnumerable<HttpRequestMock> httpRequestMocks)
        {
            handlingInfos = new List<HttpRequestMockHandlingInfo>(GetRequestMockHandlingInfos(httpRequestMocks));
        }

        public HttpRequestMockHandlingInfo Dequeue(string method, string path)
        {
            lock (handlingInfos)
            {
                var handlingInfo = handlingInfos.FirstOrDefault(i => i.RequestPattern.IsMatch(method, path) && !i.HasAttempts());
                if (handlingInfo == null)
                {
                    handlingInfo = handlingInfos.FirstOrDefault(i => i.RequestPattern.IsMatch(method, path));
                }

                handlingInfo?.IncreaseUsageCount();
                return handlingInfo;
            }
        }

        private IEnumerable<HttpRequestMockHandlingInfo> GetRequestMockHandlingInfos(IEnumerable<HttpRequestMock> requestMocks)
        {
            return requestMocks
                .Select(r => new HttpRequestMockHandlingInfo(new HttpRequestPattern(r.Method, r.PathPattern), r.Response))
                .ToArray();
        }
    }
}