namespace HttpMocks.Whens.RequestPatterns.MethodPatterns
{
    public sealed class AnyMethodPattern : IHttpRequestMethodPattern
    {
        bool IHttpRequestMethodPattern.IsMatch(string method)
        {
            return true;
        }
    }
}