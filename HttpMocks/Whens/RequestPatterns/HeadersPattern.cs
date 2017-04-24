using System.Collections.Specialized;
using HttpMocks.Whens.RequestPatterns.HeadersPatterns;

namespace HttpMocks.Whens.RequestPatterns
{
    public static class HeadersPattern
    {
        public static AnyHeadersPattern Any()
        {
            return new AnyHeadersPattern();
        }

        public static SimpleHeadersPattern Simple(NameValueCollection headers)
        {
            return new SimpleHeadersPattern(headers);
        }
    }
}