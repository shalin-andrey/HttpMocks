namespace HttpMocks.DebugLoggers
{
    public class HttpRequestPatternMatchResults
    {
        public HttpRequestPatternMatchResults(bool method, bool path, bool query, bool headers, bool content)
        {
            Method = method;
            Path = path;
            Query = query;
            Headers = headers;
            Content = content;
        }

        public bool Method { get; }
        public bool Path { get; }
        public bool Query { get; }
        public bool Headers { get; }
        public bool Content { get; }
    }
}