using System.Collections.Specialized;

namespace HttpMocks.Whens.RequestPatterns.QueryPatterns
{
    public static class QueryPattern
    {
        public static AnyQueryPattern Any()
        {
            return new AnyQueryPattern();
        }

        public static SimpleQueryPattern Simple(NameValueCollection query)
        {
            return new SimpleQueryPattern(query);
        }
    }
}