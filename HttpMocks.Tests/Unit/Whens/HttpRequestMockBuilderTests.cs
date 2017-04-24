using System;
using System.Text;
using FluentAssertions;
using HttpMocks.Implementation;
using HttpMocks.Whens;
using HttpMocks.Whens.RequestPatterns;
using HttpMocks.Whens.RequestPatterns.ContentPatterns;
using NUnit.Framework;

namespace HttpMocks.Tests.Unit.Implementation.Whens
{
    public class HttpRequestMockBuilderTests : UnitTest
    {
        private HttpRequestMockBuilder httpRequestMockBuilder;

        public override void SetUp()
        {
            base.SetUp();

            httpRequestMockBuilder = new HttpRequestMockBuilder();
            httpRequestMockBuilder.Method(MethodPattern.Standart("GET"));
            httpRequestMockBuilder.Path(PathPattern.Smart("/"));
        }

        [Test]
        public void TestContentFailWhenContentBytesIsNull()
        {
            httpRequestMockBuilder
                .Invoking(x => x.Content(null))
                .ShouldThrow<ArgumentNullException>()
                .Where(x => x.Message.Contains("httpRequestContentPattern"));
        }

        [Test]
        public void TestContent()
        {
            const string contentType = "text/string";
            var contentBytes = Encoding.UTF8.GetBytes("contentBytes");
            httpRequestMockBuilder.Content(contentBytes, contentType);
            var httpRequestMock = httpRequestMockBuilder.Build();

            httpRequestMock.Content.Should().BeOfType<BinaryContentPattern>();
        }

        [Test]
        public void TestContentPattern()
        {
            var contentPattern = ContentPattern.Any();

            httpRequestMockBuilder.Content(contentPattern);
            var httpRequestMock = httpRequestMockBuilder.Build();

            httpRequestMock.Content.ShouldBeEquivalentTo(contentPattern);
        }

        [Test]
        public void TestHeaderFailWhenHeadersIsEmpty()
        {
            httpRequestMockBuilder
                .Invoking(x => x.Headers(null))
                .ShouldThrow<ArgumentNullException>()
                .Where(x => x.Message.Contains("httpRequestHeadersPattern"));
        }

        [Test]
        public void TestThenResponseWhenEmpty()
        {
            httpRequestMockBuilder.ThenResponse();

            var httpRequestMock = httpRequestMockBuilder.Build();
            httpRequestMock.Response.Should().NotBeNull();
        }

        [Test]
        public void TestThenResponseWhenStatusCode()
        {
            const int statusCode = 200;
            httpRequestMockBuilder.ThenResponse(statusCode);

            var httpRequestMock = httpRequestMockBuilder.Build();
            httpRequestMock.Response.Should().NotBeNull();
            httpRequestMock.Response.StatusCode.ShouldBeEquivalentTo(statusCode);
        }

        [Test]
        public void TestThenResponseWhenInfoBuilder()
        {
            httpRequestMockBuilder.ThenResponse(x => new HttpResponseInfo());

            var httpRequestMock = httpRequestMockBuilder.Build();
            httpRequestMock.Response.Should().NotBeNull();
            httpRequestMock.Response.ResponseInfoBuilder.Should().NotBeNull();
            httpRequestMock.Response.StatusCode.ShouldBeEquivalentTo(0);
        }
    }
}