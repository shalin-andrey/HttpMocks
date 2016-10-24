using System;

namespace HttpMocks
{
    public class HttpRequestPattern
    {
        private readonly string method;
        private readonly HttpPathPattern pathPattern;

        public HttpRequestPattern(string method, string pathPattern)
        {
            this.method = method.ToLower();
            this.pathPattern = new HttpPathPattern(pathPattern);
        }

        public bool IsMatch(string method, string path)
        {
            if (string.IsNullOrWhiteSpace(method)) throw new ArgumentNullException(nameof(method));

            return this.method == method.ToLower() && pathPattern.IsMatch(path);
        }
    }
}