namespace HttpMocks.Whens.RequestPatterns
{
    public interface IHttpRequestPathPattern
    {
        bool IsMatch(string value);
    }
}