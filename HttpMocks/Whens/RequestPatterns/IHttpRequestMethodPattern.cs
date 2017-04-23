namespace HttpMocks.Whens.RequestPatterns
{
    public interface IHttpRequestMethodPattern
    {
        bool IsMatch(string method);
    }
}