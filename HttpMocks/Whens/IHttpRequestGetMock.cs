namespace HttpMocks
{
    public interface IHttpRequestGetMock : IHttpRequestMock
    {
        IHttpRequestGetMock WhenHeader(string headerName, string headerValue);
    }
}