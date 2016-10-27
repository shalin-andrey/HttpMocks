namespace HttpMocks.Whens
{
    public interface IHttpRequestDeleteMockBuilder : IHttpRequestMockBuilder
    {
        IHttpRequestGetMockBuilder WhenHeader(string headerName, string headerValue);
    }
}