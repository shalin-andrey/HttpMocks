using HttpMocks.Implementation;

namespace HttpMocks.Whens
{
    internal interface IHttpRequestMockBuilder : IHttpRequestMock
    {
        HttpRequestMock Build();
    }
}