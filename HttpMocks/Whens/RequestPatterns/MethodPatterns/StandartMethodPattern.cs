using System;

namespace HttpMocks.Whens.RequestPatterns.MethodPatterns
{
    public sealed class StandartMethodPattern : IHttpRequestMethodPattern
    {
        private readonly string methodPattern;

        public StandartMethodPattern(string methodPattern)
        {
            this.methodPattern = methodPattern;
        }

        bool IHttpRequestMethodPattern.IsMatch(string method)
        {
            return string.Equals(methodPattern, method, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}