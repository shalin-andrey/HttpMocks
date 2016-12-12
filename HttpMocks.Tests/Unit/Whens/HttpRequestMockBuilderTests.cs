﻿using System;
using System.Text;
using FluentAssertions;
using HttpMocks.Implementation;
using HttpMocks.Whens;
using NUnit.Framework;

namespace HttpMocks.Tests.Unit.Implementation.Whens
{
    public class HttpRequestMockBuilderTests : UnitTest
    {
        private HttpRequestMockBuilder httpRequestMockBuilder;

        public override void SetUp()
        {
            base.SetUp();

            httpRequestMockBuilder = new HttpRequestMockBuilder("GET", "/");
        }

        [Test]
        public void TestContentFailWhenContentBytesIsNull()
        {
            httpRequestMockBuilder
                .Invoking(x => x.Content(null, null))
                .ShouldThrow<ArgumentNullException>()
                .Where(x => x.Message.Contains("contentBytes"));
        }

        [Test]
        public void TestContent()
        {
            Func<byte[]> contentBytesFunc = () => Encoding.UTF8.GetBytes("contentBytes");
            const string contentType = "text/string";

            httpRequestMockBuilder.Content(contentBytesFunc(), contentType);
            var httpRequestMock = httpRequestMockBuilder.Build();

            httpRequestMock.Content.Bytes.ShouldBeEquivalentTo(contentBytesFunc());
            httpRequestMock.Content.Type.ShouldBeEquivalentTo(contentType);
        }

        [Test]
        public void TestHeaderFailWhenHeaderNameIsEmpty()
        {
            httpRequestMockBuilder
                .Invoking(x => x.Header(null, "headerValue"))
                .ShouldThrow<ArgumentNullException>()
                .Where(x => x.Message.Contains("headerName"));

            httpRequestMockBuilder
                .Invoking(x => x.Header(string.Empty, "headerValue"))
                .ShouldThrow<ArgumentNullException>()
                .Where(x => x.Message.Contains("headerName"));
        }

        [Test]
        public void TestHeader()
        {
            const string headerName = "headerName1";
            const string headervalue1 = "headerValue1";

            httpRequestMockBuilder.Header(headerName, "headerValue1");

            var httpRequestMock = httpRequestMockBuilder.Build();
            httpRequestMock.Headers.Keys.Count.ShouldBeEquivalentTo(1);
            httpRequestMock.Headers[headerName].ShouldBeEquivalentTo(headervalue1);
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