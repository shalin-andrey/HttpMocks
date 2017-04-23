namespace HttpMocks.Whens.RequestPatterns.ContentPatterns
{
    public sealed class AnyContentPattern : IHttpRequestContentPattern
    {
        bool IHttpRequestContentPattern.IsMatch(byte[] contentBytes, string contentType)
        {
            return true;
        }
    }
}