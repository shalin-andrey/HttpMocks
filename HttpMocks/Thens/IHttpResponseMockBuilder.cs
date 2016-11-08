namespace HttpMocks.Thens
{
    internal interface IHttpResponseMockBuilder : IHttpResponseMock
    {
        HttpResponseMock Build();
    }
}