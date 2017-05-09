using System;
using System.Collections.Generic;
using System.Linq;
using HttpMocks.Implementation;
using HttpMocks.Whens;
using HttpMocks.Whens.RequestPatterns;

namespace HttpMocks
{
    internal class HttpMock : IHttpMock
    {
        private readonly IStartedHttpMock startedHttpMock;
        private readonly List<IHttpRequestMockBuilder> internalHttpRequestMockBuilders;
        
        internal HttpMock(IStartedHttpMock startedHttpMock)
        {
            this.startedHttpMock = startedHttpMock;
            internalHttpRequestMockBuilders = new List<IHttpRequestMockBuilder>();
        }

        public Uri MockUri => startedHttpMock.MockUrl;

        public IHttpRequestMock WhenRequestGet(string path = null)
        {
            return WhenRequestGet(Convert(path));
        }

        public IHttpRequestMock WhenRequestGet(IHttpRequestPathPattern pathPattern)
        {
            return CreateMockBuilder()
                .Method("GET")
                .Path(pathPattern);
        }

        public IHttpRequestMock WhenRequestPost(string path = null)
        {
            return WhenRequestGet(Convert(path));
        }

        public IHttpRequestMock WhenRequestPost(IHttpRequestPathPattern pathPattern)
        {
            return CreateMockBuilder()
                .Method("POST")
                .Path(pathPattern);
        }

        public IHttpRequestMock WhenRequestPut(string path = null)
        {
            return WhenRequestGet(Convert(path));
        }

        public IHttpRequestMock WhenRequestPut(IHttpRequestPathPattern pathPattern)
        {
            return CreateMockBuilder()
                .Method("PUT")
                .Path(pathPattern);
        }

        public IHttpRequestMock WhenRequestDelete(string path = null)
        {
            return WhenRequestGet(Convert(path));
        }

        public IHttpRequestMock WhenRequestDelete(IHttpRequestPathPattern pathPattern)
        {
            return CreateMockBuilder()
                .Method("DELETE")
                .Path(pathPattern);
        }

        public IHttpRequestMock WhenRequestPatch(string path = null)
        {
            return WhenRequestGet(Convert(path));
        }

        public IHttpRequestMock WhenRequestPatch(IHttpRequestPathPattern pathPattern)
        {
            return CreateMockBuilder()
                .Method("PATCH")
                .Path(pathPattern);
        }

        public IHttpRequestMock WhenRequest()
        {
            return CreateMockBuilder();
        }

        public void Run()
        {
            var httpRequestMocks = internalHttpRequestMockBuilders.Select(b => b.Build()).ToArray();
            startedHttpMock.AppendMocks(httpRequestMocks);
        }

        public void Dispose()
        {
            Run();
        }

        private IHttpRequestPathPattern Convert(string path)
        {
            return path == null
                ? PathPattern.Any() as IHttpRequestPathPattern
                : PathPattern.Smart(path);
        }

        private HttpRequestMockBuilder CreateMockBuilder()
        {
            var requestMockBuilder = new HttpRequestMockBuilder();
            internalHttpRequestMockBuilders.Add(requestMockBuilder);
            return requestMockBuilder;
        }
    }
}