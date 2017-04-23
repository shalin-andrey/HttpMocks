using System.Collections.Specialized;
using System.Linq;

namespace HttpMocks.Whens.RequestPatterns.HeadersPatterns
{
    public sealed class SimpleHeadersPattern : IHttpRequestHeadersPattern
    {
        private readonly NameValueCollection headersPattern;

        public SimpleHeadersPattern(NameValueCollection headersPattern)
        {
            this.headersPattern = headersPattern;
        }

        bool IHttpRequestHeadersPattern.IsMatch(NameValueCollection headers)
        {
            return headersPattern
                .AllKeys
                .All(headerName => headers[headerName] == headersPattern[headerName]);
        }
    }
}