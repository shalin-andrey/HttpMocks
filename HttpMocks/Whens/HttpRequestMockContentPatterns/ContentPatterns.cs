using System;

namespace HttpMocks.Whens.HttpRequestMockContentPatterns
{
    public static class ContentPatterns
    {
        public static Base64ContentPattern Base64(string base64StringPattern, string contentTypePattern = null)
        {
            if (base64StringPattern == null) throw new ArgumentNullException(nameof(base64StringPattern));
            return Base64ContentPattern.Create(base64StringPattern, contentTypePattern);
        }

        public static Base64ContentPattern Base64(Func<string, string, bool> customApply)
        {
            if (customApply == null) throw new ArgumentNullException(nameof(customApply));
            return Base64ContentPattern.Create(customApply);
        }

        public static Base64ContentPattern Base64(Func<string, bool> customApply)
        {
            if (customApply == null) throw new ArgumentNullException(nameof(customApply));
            return Base64ContentPattern.Create((base64String, contentType) => customApply(base64String));
        }

        public static BinaryContentPattern Binary(byte[] binaryContentPattern, string contentTypePattern = null)
        {
            if (binaryContentPattern == null) throw new ArgumentNullException(nameof(binaryContentPattern));
            return BinaryContentPattern.Create(binaryContentPattern, contentTypePattern);
        }

        public static AnyContentPattern Any()
        {
            return new AnyContentPattern();
        }
    }
}