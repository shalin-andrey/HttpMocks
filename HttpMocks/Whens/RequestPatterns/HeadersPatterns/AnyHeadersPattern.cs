using System.Collections.Specialized;

namespace HttpMocks.Whens.RequestPatterns.HeadersPatterns
{
    public sealed class AnyHeadersPattern : IHttpRequestHeadersPattern
    {
        bool IHttpRequestHeadersPattern.IsMatch(NameValueCollection query)
        {
            return true;
        }
    }
}