using HttpMocks.Thens;

namespace HttpMocks.Implementation
{
    internal class HttpRequestMockHandlingInfo
    {
        public HttpRequestMockHandlingInfo(HttpRequestPattern requestPattern, HttpResponseMock responseMock)
        {
            RequestPattern = requestPattern;
            ResponseMock = responseMock;
        }

        public HttpRequestPattern RequestPattern { get; }
        public HttpResponseMock ResponseMock { get; }
    }
}