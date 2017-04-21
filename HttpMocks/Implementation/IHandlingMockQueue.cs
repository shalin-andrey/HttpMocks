namespace HttpMocks.Implementation
{
    internal interface IHandlingMockQueue
    {
        HttpRequestMockHandlingInfo Dequeue(HttpRequestInfo httpRequestInfo);
    }
}