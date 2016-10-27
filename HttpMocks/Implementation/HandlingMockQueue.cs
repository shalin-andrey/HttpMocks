using System.Collections.Generic;
using System.Linq;
using HttpMocks.Thens;

namespace HttpMocks.Implementation
{
    internal class HandlingMockQueue
    {
        private readonly List<HttpRequestMockHandlingInfo> handlingInfos;

        public HandlingMockQueue(IEnumerable<HttpRequestMock> httpRequestMocks)
        {
            handlingInfos = new List<HttpRequestMockHandlingInfo>(GetRequestMockHandlingInfos(httpRequestMocks));
        }

        public HttpResponseMock Dequeue(string method, string path)
        {
            var handlingInfo = handlingInfos.FirstOrDefault(i => i.RequestPattern.IsMatch(method, path));
            if (handlingInfo != null && handlingInfo.ResponseMock.Count == 0)
            {
                handlingInfos.Remove(handlingInfo);
            }
            return handlingInfo?.ResponseMock;
        }

        private IEnumerable<HttpRequestMockHandlingInfo> GetRequestMockHandlingInfos(IEnumerable<HttpRequestMock> requestMocks)
        {
            return requestMocks
                .Select(r => new HttpRequestMockHandlingInfo(new HttpRequestPattern(r.Method, r.PathPattern), r.Response))
                .ToArray();
        }
    }
}