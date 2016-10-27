using System;
using HttpMocks.Thens;
using HttpMocks.Whens;

namespace HttpMocks
{
    public interface IHttpMock : IDisposable
    {
        IHttpRequestGetMockBuilder WhenRequestGet();
        IHttpRequestGetMockBuilder WhenRequestGet(string path);
        IHttpRequestPostMockBuilder WhenRequestPost(string path);
        void Run();

        Uri MockUri { get; }
    }
}