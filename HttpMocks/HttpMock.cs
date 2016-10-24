using System.Collections.Generic;
using System.Linq;
using HttpMocks.Whens;

namespace HttpMocks
{
    public class HttpMock
    {
        private readonly List<IInternalHttpRequestMockBuilder> internalHttpRequestMockBuilders;

        public static HttpMock New(string prefix)
        {
            return new HttpMock(prefix);
        }

        private HttpMock(string prefix)
        {
            Prefix = prefix;
            internalHttpRequestMockBuilders = new List<IInternalHttpRequestMockBuilder>();
        }

        internal string Prefix { get; }

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
            var requestMock = new HttpRequestWihtoutContentMockBuilder(path);
            internalHttpRequestMockBuilders.Add(requestMock);
            return requestMock;
        }
    }
}