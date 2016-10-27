using System;

namespace HttpMocks.Implementation
{
    public class HttpRequestPattern
    {
        private readonly string methodPattern;
        private readonly HttpPathPattern pathPattern;

        public HttpRequestPattern(string method, string pathPattern)
        {
            methodPattern = method.ToLower();
            this.pathPattern = new HttpPathPattern(pathPattern);
        }

        public bool IsMatch(string method, string path)
        {
            if (string.IsNullOrWhiteSpace(method)) throw new ArgumentNullException(nameof(method));

            return string.Equals(methodPattern, method, StringComparison.OrdinalIgnoreCase) && pathPattern.IsMatch(path);
        }
    }
}