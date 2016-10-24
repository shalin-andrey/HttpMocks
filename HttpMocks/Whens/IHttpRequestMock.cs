namespace HttpMocks
{
    public interface IHttpRequestMock
    {
        IHttpResponseMock ThenResponse(int statusCode);
    }
}