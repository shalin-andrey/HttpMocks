namespace HttpMocks
{
    public interface IHttpResponseMock
    {
        IHttpResponseMock ThenContent(byte[] contentBytes, string contentType);
        IHttpResponseMock ThenHeader(string headerName, string headerValue);
    }
}