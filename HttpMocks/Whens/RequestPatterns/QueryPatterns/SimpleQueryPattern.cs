using System.Collections.Specialized;
using System.Linq;

namespace HttpMocks.Whens.RequestPatterns.QueryPatterns
{
    public sealed class SimpleQueryPattern : IHttpRequestQueryPattern
    {
        private readonly NameValueCollection queryPattern;

        public SimpleQueryPattern(NameValueCollection queryPattern)
        {
            this.queryPattern = queryPattern;
        }

        bool IHttpRequestQueryPattern.IsMatch(NameValueCollection query)
        {
            if (query.Count != queryPattern.Count)
            {
                return false;
            }

            return queryPattern
                .AllKeys
                .All(queryParameterName => query[queryParameterName] == queryPattern[queryParameterName]);
        }
    }
}