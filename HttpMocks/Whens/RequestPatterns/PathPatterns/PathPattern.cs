namespace HttpMocks.Whens.RequestPatterns.PathPatterns
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