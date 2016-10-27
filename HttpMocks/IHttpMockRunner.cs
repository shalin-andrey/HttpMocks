namespace HttpMocks
{
    public interface IHttpMockRunner
    {
        void RunMock(HttpMock httpMock);
        void VerifyAll();
    }
}