using System.Collections.Specialized;
using HttpMocks.Whens.RequestPatterns;

namespace HttpMocks.Whens
{
    public static class HttpRequestMockExtensions
    {
        public static IHttpRequestMock Content(this IHttpRequestMock httpRequestMock, byte[] contentBytes, string contentType = null)
        {
            return httpRequestMock.Content(ContentPattern.Binary(contentBytes, contentType));
        }

        public static IHttpRequestMock Headers(this IHttpRequestMock httpRequestMock, NameValueCollection headers)
        {
            return httpRequestMock.Headers(HeadersPattern.Simple(headers));
        }

        public static IHttpRequestMock Query(this IHttpRequestMock httpRequestMock, NameValueCollection headers)
        {
            return httpRequestMock.Query(QueryPattern.Simple(headers));
        }

        public static IHttpRequestMock Path(this IHttpRequestMock httpRequestMock, string path)
        {
            return httpRequestMock.Path(PathPattern.Smart(path));
        }

        public static IHttpRequestMock Method(this IHttpRequestMock httpRequestMock, string method)
        {
            return httpRequestMock.Method(MethodPattern.Standart(method));
        }

        public static IHttpRequestMock Method(this IHttpRequestMock httpRequestMock, HttpRequestMockMethod method)
        {
            return httpRequestMock.Method(MethodPattern.Standart(method));
        }
    }
}