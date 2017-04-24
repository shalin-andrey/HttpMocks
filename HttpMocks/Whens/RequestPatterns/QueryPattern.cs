using System.Collections.Specialized;
using HttpMocks.Whens.RequestPatterns.QueryPatterns;

namespace HttpMocks.Whens.RequestPatterns
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