using System;
using HttpMocks.Whens;

namespace HttpMocks
{
    public interface IHttpMock : IDisposable
    {
        IHttpRequestMock WhenRequestGet();
        IHttpRequestMock WhenRequestGet(string path);
        IHttpRequestMock WhenRequestPost(string path);
        void Run();

        Uri MockUri { get; }
    }
}