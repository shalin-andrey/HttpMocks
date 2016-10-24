using HttpMocks.Thens;

namespace HttpMocks.Whens
{
    public interface IHttpRequestMockBuilder
    {
        IHttpResponseMockBuilder ThenResponse(int statusCode);
    }
}