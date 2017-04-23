using System.Collections.Specialized;

namespace HttpMocks.Whens.RequestPatterns.QueryPatterns
{
    public sealed class AnyQueryPattern : IHttpRequestQueryPattern
    {
        bool IHttpRequestQueryPattern.IsMatch(NameValueCollection query)
        {
            return true;
        }
    }
}