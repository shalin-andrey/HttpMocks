using System;
using System.Threading.Tasks;
using HttpMocks.Implementation;
using HttpMocks.Thens;
using HttpMocks.Whens.RequestPatterns;

namespace HttpMocks.Whens
{
    public interface IHttpRequestMock
    {
        IHttpRequestMock Method(IHttpRequestMethodPattern httpRequestMethodPattern);
        IHttpRequestMock Headers(IHttpRequestHeadersPattern httpRequestHeadersPattern);
        IHttpRequestMock Path(IHttpRequestPathPattern httpRequestPathPattern);
        IHttpRequestMock Query(IHttpRequestQueryPattern httpRequestQueryPattern);
        IHttpRequestMock Content(IHttpRequestContentPattern httpRequestContentPattern);

        IHttpResponseMock ThenResponse(int statusCode);
        IHttpResponseMock ThenResponse();
        ICustomHttpResponseMock ThenResponse(Func<HttpRequest, HttpResponse> responseInfoBuilder);
        ICustomHttpResponseMock ThenResponse(Func<HttpRequest, Task<HttpResponse>> asyncResponseInfoBuilder);
    }
}