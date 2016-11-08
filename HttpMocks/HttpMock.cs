using System;
using System.Collections.Generic;
using System.Linq;
using HttpMocks.Whens;

namespace HttpMocks
{
    internal class HttpMock : IHttpMock
    {
        private readonly IHttpMockRunner httpMockRunner;
        private readonly List<IHttpRequestMockBuilder> internalHttpRequestMockBuilders;
        
        internal HttpMock(IHttpMockRunner httpMockRunner, Uri mockUri)
        {
            this.httpMockRunner = httpMockRunner;
            MockUri = mockUri;
            internalHttpRequestMockBuilders = new List<IHttpRequestMockBuilder>();
        }

        public Uri MockUri { get; }

        public IHttpRequestMock WhenRequestGet()
        {
            return WhenRequestGet(string.Empty);
        }

        public IHttpRequestMock WhenRequestGet(string path)
        {
            var requestMock = new HttpRequestMockBuilder("GET", path);
            internalHttpRequestMockBuilders.Add(requestMock);
            return requestMock;
        }

        public IHttpRequestMock WhenRequestPost(string path)
        {
            var requestMock = new HttpRequestMockBuilder("POST", path);
            internalHttpRequestMockBuilders.Add(requestMock);
            return requestMock;
        }

        public void Run()
        {
            var httpRequestMocks = internalHttpRequestMockBuilders.Select(b => b.Build()).ToArray();
            httpMockRunner.RunMocks(MockUri, httpRequestMocks);
        }

        public void Dispose()
        {
            Run();
        }
    }
}