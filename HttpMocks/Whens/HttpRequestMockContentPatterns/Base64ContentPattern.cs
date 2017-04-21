using System;
using System.Text;

namespace HttpMocks.Whens.HttpRequestMockContentPatterns
{
    public class Base64ContentPattern : ContentPatternBase<string>
    {
        internal static Base64ContentPattern Create(string base64StringPattern, string contentTypePattern = null)
        {
            return new Base64ContentPattern((base64String, contentType) =>
            {
                if (!string.IsNullOrEmpty(contentTypePattern) && contentTypePattern != contentType)
                    return false;
                return string.Equals(base64String, base64StringPattern, StringComparison.InvariantCultureIgnoreCase);
            });
        }
        
        internal static Base64ContentPattern Create(Func<string, string, bool> contentIsMatch)
        {
            return new Base64ContentPattern(contentIsMatch);
        }

        private Base64ContentPattern(Func<string, string, bool> contentIsMatch)
            : base(contentIsMatch)
        {
        }

        protected override string ParseContentFromBytes(byte[] contentBytes)
        {
            return Encoding.ASCII.GetString(contentBytes);
        }
    }
}