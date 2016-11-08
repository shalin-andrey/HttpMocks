using System;
using HttpMocks.Implementation;
using HttpMocks.Thens;

namespace HttpMocks.Whens
{
    public interface IHttpRequestMock
    {
        IHttpRequestMock Header(string headerName, string headerValue);
        IHttpRequestMock Content(byte[] contentBytes, string contentType);
        IHttpResponseMock ThenResponse(int statusCode);
        IHttpResponseMock ThenResponse();
        ICustomHttpResponseMock ThenResponse(Func<HttpRequestInfo, HttpResponseInfo> responseInfoBuilder);
    }
}