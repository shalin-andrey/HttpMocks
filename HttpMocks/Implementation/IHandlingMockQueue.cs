namespace HttpMocks.Implementation
{
    internal interface IHandlingMockQueue
    {
        HttpRequestMockHandlingInfo Dequeue(HttpRequestInfo httpRequestInfo);
        void Enqueue(HttpRequestMock[] httpRequestMocks);
    }
}