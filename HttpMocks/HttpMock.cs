using System;
using System.Collections.Generic;
using System.Linq;
using HttpMocks.Implementation;
using HttpMocks.Thens;
using HttpMocks.Whens;

namespace HttpMocks
{
    public class HttpMock : IHttpMock
    {
        private readonly IHttpMockRunner httpMockRunner;
        private readonly List<IInternalHttpRequestMockBuilder> internalHttpRequestMockBuilders;
        
        internal HttpMock(IHttpMockRunner httpMockRunner, Uri mockUri)
        {
            this.httpMockRunner = httpMockRunner;
            MockUri = mockUri;
            internalHttpRequestMockBuilders = new List<IInternalHttpRequestMockBuilder>();
        }

        public Uri MockUri { get; }

        internal HttpRequestMock[] Build()
        {
            return internalHttpRequestMockBuilders.Select(b => b.Build()).ToArray();
        }

        public IHttpRequestGetMockBuilder WhenRequestGet()
        {
            return WhenRequestGet(string.Empty);
        }

        public IHttpRequestGetMockBuilder WhenRequestGet(string path)
        {
            var requestMock = new HttpRequestWithoutContentMockBuilder(path);
            internalHttpRequestMockBuilders.Add(requestMock);
            return requestMock;
        }

        public IHttpRequestPostMockBuilder WhenRequestPost(string path)
        {
            var requestMock = new HttpRequestWithContentMockBuilder(path);
            internalHttpRequestMockBuilders.Add(requestMock);
            return requestMock;
        }

        public void Run()
        {
            httpMockRunner.RunMock(this);
        }

        public void Dispose()
        {
            Run();
        }
    }
}