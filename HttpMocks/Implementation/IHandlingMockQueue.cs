namespace HttpMocks.Implementation
{
    internal interface IHandlingMockQueue
    {
        HttpRequestMockHandlingInfo Dequeue(string method, string path);
    }
}