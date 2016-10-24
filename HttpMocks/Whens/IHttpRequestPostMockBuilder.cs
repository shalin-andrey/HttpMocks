namespace HttpMocks.Thens
{
    public interface IHttpRequestPostMockBuilder : IHttpRequestMock
    {
        IHttpRequestPostMockBuilder WhenHeader(string headerName, string headerValue);
        IHttpRequestPostMockBuilder WhenContent(byte[] contentBytes, string contentType);
    }
}