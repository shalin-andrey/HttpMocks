using FluentAssertions;
using HttpMocks.Implementation;
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
            var httpRequestPattern = new HttpRequestPattern(method, pathPattern);
            httpRequestPattern.IsMatch(expectedMethod, pathPattern).ShouldBeEquivalentTo(expected);
        }
    }
}