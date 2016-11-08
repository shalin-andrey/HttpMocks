using HttpMocks.Thens;

namespace HttpMocks.Implementation
{
    internal class HttpRequestMockHandlingInfo
    {
        public HttpRequestMockHandlingInfo(HttpRequestPattern requestPattern, HttpResponseMock responseMock)
        {
            RequestPattern = requestPattern;
            ResponseMock = responseMock;
            UsageCount = 0;
        }

        public void IncreaseUsageCount()
        {
            UsageCount++;
        }

        public bool HasAttempts()
        {
            return ResponseMock.RepeatCount > UsageCount;
        }

        public bool IsUsageCountValid()
        {
            return ResponseMock.RepeatCount >= UsageCount;
        }

        public HttpRequestPattern RequestPattern { get; }
        public HttpResponseMock ResponseMock { get; }
        public int UsageCount { get; private set; }
    }
}