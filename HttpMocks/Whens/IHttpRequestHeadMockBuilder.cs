namespace HttpMocks.Whens
{
    public interface IHttpRequestHeadMockBuilder : IHttpRequestMockBuilder
    {
        IHttpRequestGetMockBuilder WhenHeader(string headerName, string headerValue);
    }
}