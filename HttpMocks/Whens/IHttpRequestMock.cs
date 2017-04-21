using System;
using System.Threading.Tasks;
using HttpMocks.Implementation;
using HttpMocks.Thens;

namespace HttpMocks.Whens
{
    public interface IHttpRequestMock
    {
        IHttpRequestMock Header(string headerName, string headerValue);
        IHttpRequestMock Content(byte[] contentBytes, string contentType);
        IHttpRequestMock Content(IHttpRequestMockContentPattern httpRequestMockContentPattern);
        IHttpResponseMock ThenResponse(int statusCode);
        IHttpResponseMock ThenResponse();
        ICustomHttpResponseMock ThenResponse(Func<HttpRequestInfo, HttpResponseInfo> responseInfoBuilder);
        ICustomHttpResponseMock ThenResponse(Func<HttpRequestInfo, Task<HttpResponseInfo>> asyncResponseInfoBuilder);
    }
}