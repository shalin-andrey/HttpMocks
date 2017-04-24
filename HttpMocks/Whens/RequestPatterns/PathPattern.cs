using HttpMocks.Whens.RequestPatterns.PathPatterns;

namespace HttpMocks.Whens.RequestPatterns
{
    public static class PathPattern
    {
        public static AnyPathPattern Any()
        {
            return new AnyPathPattern();
        }

        public static SmartPathPattern Smart(string pathPattern)
        {
            return new SmartPathPattern(pathPattern);
        }
    }
}