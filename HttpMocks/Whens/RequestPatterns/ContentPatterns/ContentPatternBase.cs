using System;

namespace HttpMocks.Whens.RequestPatterns.ContentPatterns
{
    public abstract class ContentPatternBase<T> : IHttpRequestContentPattern
    {
        private readonly Func<T, string, bool> contentIsMatch;

        protected ContentPatternBase(Func<T, string, bool> contentIsMatch)
        {
            this.contentIsMatch = contentIsMatch;
        }

        bool IHttpRequestContentPattern.IsMatch(byte[] contentBytes, string contentType)
        {
            var parsedContent = ParseContentFromBytes(contentBytes);
            return contentIsMatch(parsedContent, contentType);
        }

        protected abstract T ParseContentFromBytes(byte[] contentBytes);
    }
}