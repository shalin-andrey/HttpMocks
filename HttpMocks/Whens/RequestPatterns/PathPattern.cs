using HttpMocks.Whens.RequestPatterns.PathPatterns;

namespace HttpMocks.Whens.RequestPatterns
{
    public static class PathPattern
    {
        public static AnyPathPattern Any()
        {
            return new AnyPathPattern();
        }

        /// <summary>
        /// Create smart path pattern. Format example, /users/@guid.
        /// Available patterns: @guid - GUID, @str - string.
        /// </summary>
        /// <param name="pathPattern">Path pattern</param>
        /// <returns>Smart path pattern</returns>
        public static SmartPathPattern Smart(string pathPattern)
        {
            return new SmartPathPattern(pathPattern);
        }
    }
}