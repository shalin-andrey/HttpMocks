namespace HttpMocks.Implementation
{
    internal interface IHandlingMockQueue
    {
        HttpRequestMockHandlingInfo Dequeue(HttpRequest httpRequest);
        void Enqueue(HttpRequestMock[] httpRequestMocks);
        HttpRequestMockHandlingInfo[] GetNotActual();
    }
}