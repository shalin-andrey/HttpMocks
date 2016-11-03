using System;
using HttpMocks.Implementation;

namespace HttpMocks
{
    public interface IHttpMockRunner
    {
        void RunMocks(Uri mockUrl, HttpRequestMock[] httpRequestMocks);
        void VerifyAll();
    }
}