using HttpMocks.Implementation;

namespace HttpMocks
{
    internal interface IHttpMockRunner
    {
        IStartedHttpMock RunMocks(IMockUrlEnumerator mockUrlEnumerator, IHandlingMockQueue handlingMockQueue);
    }
}