using System.Collections.Specialized;

namespace HttpMocks.Whens.RequestPatterns.HeadersPatterns
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