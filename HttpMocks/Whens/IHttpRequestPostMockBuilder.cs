using HttpMocks.Whens;

namespace HttpMocks.Thens
{
    public interface IHttpRequestPostMockBuilder : IHttpRequestMockBuilder
    {
        IHttpRequestPostMockBuilder WhenHeader(string headerName, string headerValue);
        IHttpRequestPostMockBuilder WhenContent(byte[] contentBytes, string contentType);
    }
}