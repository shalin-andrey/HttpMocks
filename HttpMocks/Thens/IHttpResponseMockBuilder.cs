namespace HttpMocks.Thens
{
    public interface IHttpResponseMockBuilder
    {
        IHttpResponseMockBuilder ThenContent(byte[] contentBytes, string contentType);
        IHttpResponseMockBuilder ThenHeader(string headerName, string headerValue);
    }
}