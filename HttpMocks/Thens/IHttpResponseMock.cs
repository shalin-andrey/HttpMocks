namespace HttpMocks.Thens
{
    public interface IHttpResponseMock
    {
        IHttpResponseMock StatusCode(int statusCode);
        IHttpResponseMock Content(byte[] contentBytes, string contentType);
        IHttpResponseMock Header(string headerName, string headerValue);
        IHttpResponseMock Repeat(int count);
        IHttpResponseMock RepeatAny();
    }
}