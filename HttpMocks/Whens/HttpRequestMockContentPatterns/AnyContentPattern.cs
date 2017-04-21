namespace HttpMocks.Whens.HttpRequestMockContentPatterns
{
    public class AnyContentPattern : IHttpRequestMockContentPattern
    {
        public bool IsMatch(byte[] contentBytes, string contentType)
        {
            return true;
        }
    }
}