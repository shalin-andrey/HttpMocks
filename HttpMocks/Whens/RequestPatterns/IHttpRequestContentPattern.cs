namespace HttpMocks.Whens.RequestPatterns
{
    public interface IHttpRequestContentPattern
    {
        bool IsMatch(byte[] contentBytes, string contentType);
    }
}