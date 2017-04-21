using FluentAssertions;
using HttpMocks.Implementation;
using HttpMocks.Whens;
using HttpMocks.Whens.HttpRequestMockContentPatterns;
using Moq;
using NUnit.Framework;

namespace HttpMocks.Tests.Unit.Implementation
{
    public class HttpRequestPatternTests : UnitTest
    {
        [Test]
        [TestCase("GET", "GET", true)]
        [TestCase("GET", "get", true)]
        [TestCase("GET", "gEt", true)]
        [TestCase("GET", "gEtd", false)]
        public void TestIsMatch(string method, string expectedMethod, bool expected)
        {
            const string pathPattern = "/";
            var httpRequestPattern = new HttpRequestPattern(method, pathPattern, ContentPatterns.Any());
            var httpRequestInfo = new HttpRequestInfo
            {
                Method = expectedMethod,
                Path = pathPattern
            };
            httpRequestPattern.IsMatch(httpRequestInfo).ShouldBeEquivalentTo(expected);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void TestIsMatchWithContentPattern(bool isMatchResult)
        {
            var contnetPatternMock = NewMock<IHttpRequestMockContentPattern>();
            const string method = "post";
            const string path = "/";
            var contentBytes = new byte[0];
            const string contentType = "application/json";

            contnetPatternMock.Setup(x => x.IsMatch(contentBytes, contentType)).Returns(isMatchResult);

            var httpRequestPattern = new HttpRequestPattern(method, path, contnetPatternMock.Object);
            var httpRequestInfo = new HttpRequestInfo
            {
                Method = method,
                Path = path,
                ContentBytes = contentBytes,
                ContentType = contentType
            };
            
            httpRequestPattern.IsMatch(httpRequestInfo).ShouldBeEquivalentTo(isMatchResult);
        }
    }
}