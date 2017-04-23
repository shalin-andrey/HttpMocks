using System;
using System.Linq;

namespace HttpMocks.Whens.RequestPatterns.ContentPatterns
{
    public sealed class BinaryContentPattern : ContentPatternBase<byte[]>
    {
        internal static BinaryContentPattern Create(byte[] bytesPattern, string contentTypePattern = null)
        {
            return new BinaryContentPattern((bytes, contentType) =>
            {
                if (!string.IsNullOrEmpty(contentTypePattern) && contentTypePattern != contentType)
                {
                    return false;
                }

                if (bytesPattern.Length != bytes.Length)
                {
                    return false;
                }

                return !bytesPattern.Where((t, i) => t != bytes[i]).Any();
            });
        }

        private BinaryContentPattern(Func<byte[], string, bool> contentIsMatch) 
            : base(contentIsMatch)
        {
        }

        protected override byte[] ParseContentFromBytes(byte[] contentBytes)
        {
            return contentBytes;
        }
    }
}