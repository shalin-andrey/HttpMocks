namespace HttpMocks.Thens
{
    public interface ICustomHttpResponseMock
    {
        IHttpResponseMock Repeat(int count);
        IHttpResponseMock RepeatAny();
    }
}