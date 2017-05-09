using System;
using System.Threading.Tasks;
using HttpMocks.Implementation;

namespace HttpMocks.Thens
{
    internal class HttpResponseMockBuilder : IHttpResponseMockBuilder, ICustomHttpResponseMock
    {
        private readonly HttpResponseMock httpResponseMock;

        public HttpResponseMockBuilder(int statusCode, Func<HttpRequest, Task<HttpResponse>> asyncResponseInfoBuilder = null)
        {
            httpResponseMock = new HttpResponseMock
            {
                StatusCode = statusCode,
                Content = HttpResponseMockContent.Empty,
                ResponseInfoBuilder = asyncResponseInfoBuilder
            };
        }

        public HttpResponseMockBuilder()
            : this(0)
        {
        }

        public HttpResponseMockBuilder(Func<HttpRequest, Task<HttpResponse>> asyncResponseInfoBuilder)
            : this(0, asyncResponseInfoBuilder)
        {
        }

        public IHttpResponseMock StatusCode(int statusCode)
        {
            httpResponseMock.StatusCode = statusCode;

            return this;
        }

        public IHttpResponseMock Content(byte[] contentBytes, string contentType)
        {
            if (contentBytes == null) throw new ArgumentNullException(nameof(contentBytes));

            httpResponseMock.Content = new HttpResponseMockContent(contentBytes, contentType);

            return this;
        }

        public IHttpResponseMock Header(string headerName, string headerValue)
        {
            if (string.IsNullOrEmpty(headerName)) throw new ArgumentNullException(nameof(headerName));

            httpResponseMock.Headers[headerName] = headerValue;

            return this;
        }

        public IHttpResponseMock Repeat(int count)
        {
            httpResponseMock.RepeatCount = count;

            return this;
        }

        public IHttpResponseMock RepeatAny()
        {
            httpResponseMock.RepeatCount = int.MaxValue;

            return this;
        }

        public HttpResponseMock Build()
        {
            return httpResponseMock;
        }
    }
}