using HttpMocks.Verifications;

namespace HttpMocks
{
    internal abstract class HttpRequestMock
    {
        protected HttpRequestMockModel

        protected HttpRequestMock(string method, string pathPattern)
        {
            Method = method;
            PathPattern = pathPattern;
            Response = new HttpResponseMock(200);
        }

        public string Method { get; private set; }
        public string PathPattern { get; private set; }
        public HttpResponseMock Response { get; private set; }

        public IHttpResponseMock ThenResponse(int statusCode)
        {
            return Response = new HttpResponseMock(statusCode);
        }


    }
}