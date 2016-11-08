using System;
using System.Text;
using FluentAssertions;
using HttpMocks.Thens;
using NUnit.Framework;

namespace HttpMocks.Tests.Unit.Implementation.Thens
{
    public class HttpResponseMockBuilderTests : UnitTest
    {
        [Test]
        public void TestHeader()
        {
            const string headerValue = "headerValue1";
            const string headerName = "headerName1";

            var httpResponseMockBuilder = new HttpResponseMockBuilder();
            httpResponseMockBuilder.Header(headerName, headerValue);
            var httpResponseMock = httpResponseMockBuilder.Build();

            httpResponseMock.Headers.Should().NotBeNull();
            httpResponseMock.Headers.Keys.Count.ShouldBeEquivalentTo(1);
            httpResponseMock.Headers[headerName].ShouldBeEquivalentTo(headerValue);
        }

        [Test]
        public void TestHeaderFailWhenHeaderNameEmpry()
        {
            const string headerValue = "headerValue1";

            var httpResponseMockBuilder = new HttpResponseMockBuilder();
            httpResponseMockBuilder
                .Invoking(x => x.Header(null, headerValue))
                .ShouldThrow<ArgumentNullException>()
                .Where(x => x.Message.Contains("headerName"));

            httpResponseMockBuilder
                .Invoking(x => x.Header(string.Empty, headerValue))
                .ShouldThrow<ArgumentNullException>()
                .Where(x => x.Message.Contains("headerName"));
        }

        [Test]
        public void TestContentFailWhenContentBytesIsNull()
        {
            var httpResponseMockBuilder = new HttpResponseMockBuilder();
            httpResponseMockBuilder
                .Invoking(x => x.Content(null, null))
                .ShouldThrow<ArgumentNullException>()
                .Where(x => x.Message.Contains("contentBytes"));
        }

        [Test]
        public void TestContent()
        {
            Func<byte[]> contentBytesFunc = () => Encoding.UTF8.GetBytes("contentBytes");
            const string contentType = "text/string";

            var httpResponseMockBuilder = new HttpResponseMockBuilder();
            httpResponseMockBuilder.Content(contentBytesFunc(), contentType);
            var httpRequestMock = httpResponseMockBuilder.Build();

            httpRequestMock.Content.Bytes.ShouldBeEquivalentTo(contentBytesFunc());
            httpRequestMock.Content.Type.ShouldBeEquivalentTo(contentType);
        }

        [Test]
        public void TestRepeatWhenDefault()
        {
            var httpResponseMockBuilder = new HttpResponseMockBuilder();
            var httpRequestMock = httpResponseMockBuilder.Build();

            httpRequestMock.RepeatCount.ShouldBeEquivalentTo(1);
        }

        [Test]
        public void TestRepeat()
        {
            const int repeatCount = 4;
            var httpResponseMockBuilder = new HttpResponseMockBuilder();
            httpResponseMockBuilder.Repeat(repeatCount);
            var httpRequestMock = httpResponseMockBuilder.Build();

            httpRequestMock.RepeatCount.ShouldBeEquivalentTo(repeatCount);
        }

        [Test]
        public void TestRepeatAny()
        {
            var httpResponseMockBuilder = new HttpResponseMockBuilder();
            httpResponseMockBuilder.RepeatAny();
            var httpRequestMock = httpResponseMockBuilder.Build();

            httpRequestMock.RepeatCount.ShouldBeEquivalentTo(int.MaxValue);
        }

        [Test]
        public void TestStatusCode()
        {
            const int statusCode = 300;

            var httpResponseMockBuilder = new HttpResponseMockBuilder();
            httpResponseMockBuilder.StatusCode(statusCode);
            var httpRequestMock = httpResponseMockBuilder.Build();

            httpRequestMock.StatusCode.ShouldBeEquivalentTo(statusCode);
        }

        [Test]
        public void TestStatusCodeWhenDefault()
        {
            var httpResponseMockBuilder = new HttpResponseMockBuilder();
            var httpRequestMock = httpResponseMockBuilder.Build();

            httpRequestMock.StatusCode.ShouldBeEquivalentTo(0);
        }
    }
}