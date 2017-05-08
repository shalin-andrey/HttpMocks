using HttpMocks.Whens;
using HttpMocks.Whens.RequestPatterns;

namespace HttpMocks
{
    public static class IHttpMockExtensions
    {
        public static IHttpRequestMock WhenRequestGet(this IHttpMock httpMock)
        {
            return httpMock.WhenRequestGet(PathPattern.Any());
        }

        public static IHttpRequestMock WhenRequestPut(this IHttpMock httpMock)
        {
            return httpMock.WhenRequestPut(PathPattern.Any());
        }

        public static IHttpRequestMock WhenRequestDelete(this IHttpMock httpMock)
        {
            return httpMock.WhenRequestDelete(PathPattern.Any());
        }

        public static IHttpRequestMock WhenRequestPatch(this IHttpMock httpMock)
        {
            return httpMock.WhenRequestPatch(PathPattern.Any());
        }

        public static IHttpRequestMock WhenRequestPost(this IHttpMock httpMock)
        {
            return httpMock.WhenRequestPost(PathPattern.Any());
        }

        public static IHttpRequestMock WhenRequestGet(this IHttpMock httpMock, string path)
        {
            return httpMock.WhenRequestGet(PathPattern.Smart(path));
        }

        public static IHttpRequestMock WhenRequestPut(this IHttpMock httpMock, string path)
        {
            return httpMock.WhenRequestPut(PathPattern.Smart(path));
        }

        public static IHttpRequestMock WhenRequestDelete(this IHttpMock httpMock, string path)
        {
            return httpMock.WhenRequestDelete(PathPattern.Smart(path));
        }

        public static IHttpRequestMock WhenRequestPatch(this IHttpMock httpMock, string path)
        {
            return httpMock.WhenRequestPatch(PathPattern.Smart(path));
        }

        public static IHttpRequestMock WhenRequestPost(this IHttpMock httpMock, string path)
        {
            return httpMock.WhenRequestPost(PathPattern.Smart(path));
        }
    }
}