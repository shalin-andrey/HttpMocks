namespace HttpMocks.Whens
{
    public interface IHttpRequestMockContentPattern
    {
        bool IsMatch(byte[] contentBytes, string contentType);
    }
}