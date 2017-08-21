using HttpMocks.Thens;
using HttpMocks.Whens.RequestPatterns;

namespace HttpMocks.Implementation
{
    internal class HttpRequestMockHandlingInfo
    {
        public HttpRequestMockHandlingInfo(HttpRequestPattern requestPattern, HttpRequestMock requestMock)
        {
            RequestPattern = requestPattern;
            RequestMock = requestMock;
            UsageCount = 0;
        }

        public void IncreaseUsageCount()
        {
            UsageCount++;
        }

        public bool HasAttempts()
        {
            if (ResponseMock.RepeatCount == int.MaxValue)
            {
                return true;
            }

            return ResponseMock.RepeatCount > UsageCount;
        }

        public bool IsUsageCountValid()
        {
            if (ResponseMock.RepeatCount == int.MaxValue)
            {
                return true;
            }

            return ResponseMock.RepeatCount >= UsageCount;
        }

        public bool HasNotActual()
        {
            if (ResponseMock.RepeatCount == int.MaxValue)
            {
                return false;
            }

            return HasAttempts();
        }

        public HttpRequestPattern RequestPattern { get; }
        public HttpRequestMock RequestMock { get; }
        public HttpResponseMock ResponseMock => RequestMock.Response;
        public int UsageCount { get; private set; }
    }
}