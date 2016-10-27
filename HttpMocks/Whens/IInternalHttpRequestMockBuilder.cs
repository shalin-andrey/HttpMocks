using HttpMocks.Implementation;

namespace HttpMocks.Whens
{
    internal interface IInternalHttpRequestMockBuilder
    {
        HttpRequestMock Build();
    }
}