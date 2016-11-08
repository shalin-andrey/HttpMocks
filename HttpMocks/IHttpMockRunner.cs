using System;
using HttpMocks.Implementation;

namespace HttpMocks
{
    internal interface IHttpMockRunner
    {
        void RunMocks(Uri mockUrl, HttpRequestMock[] httpRequestMocks);
        void VerifyAll();
    }
}