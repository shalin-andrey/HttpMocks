using System;
using HttpMocks.Whens;

namespace HttpMocks
{
    public interface IHttpMock : IDisposable
    {
        IHttpRequestMock WhenRequestGet();
        IHttpRequestMock WhenRequestGet(string path);

        IHttpRequestMock WhenRequestPost();
        IHttpRequestMock WhenRequestPost(string path);

        IHttpRequestMock WhenRequestPut();
        IHttpRequestMock WhenRequestPut(string path);

        IHttpRequestMock WhenRequestDelete();
        IHttpRequestMock WhenRequestDelete(string path);

        IHttpRequestMock WhenRequestPatch();
        IHttpRequestMock WhenRequestPatch(string path);

        void Run();

        Uri MockUri { get; }
    }
}