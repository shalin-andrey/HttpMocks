using System;

namespace HttpMocks.Whens.HttpRequestMockContentPatterns
{
    public abstract class ContentPatternBase<T> : IHttpRequestMockContentPattern
    {
        private readonly Func<T, string, bool> contentIsMatch;

        protected ContentPatternBase(Func<T, string, bool> contentIsMatch)
        {
            this.contentIsMatch = contentIsMatch;
        }

        bool IHttpRequestMockContentPattern.IsMatch(byte[] contentBytes, string contentType)
        {
            var parsedContent = ParseContentFromBytes(contentBytes);
            return contentIsMatch(parsedContent, contentType);
        }

        protected abstract T ParseContentFromBytes(byte[] contentBytes);
    }
}