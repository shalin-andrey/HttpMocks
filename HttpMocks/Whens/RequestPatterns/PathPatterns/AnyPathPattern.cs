namespace HttpMocks.Whens.RequestPatterns.PathPatterns
{
    public sealed class AnyPathPattern : IHttpRequestPathPattern
    {
        bool IHttpRequestPathPattern.IsMatch(string value)
        {
            return true;
        }
    }
}