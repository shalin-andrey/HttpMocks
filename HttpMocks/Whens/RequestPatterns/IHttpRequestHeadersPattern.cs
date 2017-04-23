using System.Collections.Specialized;

namespace HttpMocks.Whens.RequestPatterns
{
    public interface IHttpRequestHeadersPattern
    {
        bool IsMatch(NameValueCollection headers);
    }
}