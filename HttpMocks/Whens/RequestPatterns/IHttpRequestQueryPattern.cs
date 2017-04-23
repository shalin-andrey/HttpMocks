using System.Collections.Specialized;

namespace HttpMocks.Whens.RequestPatterns
{
    public interface IHttpRequestQueryPattern
    {
        bool IsMatch(NameValueCollection query);
    }
}