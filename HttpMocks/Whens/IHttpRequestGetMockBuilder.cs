namespace HttpMocks.Whens
{
    public interface IHttpRequestGetMockBuilder : IHttpRequestMockBuilder
    {
        IHttpRequestGetMockBuilder WhenHeader(string headerName, string headerValue);
    }
}