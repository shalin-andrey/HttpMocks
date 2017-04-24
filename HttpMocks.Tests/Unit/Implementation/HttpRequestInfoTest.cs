using System.Collections.Specialized;
using FluentAssertions;
using HttpMocks.Implementation;
using NUnit.Framework;

namespace HttpMocks.Tests.Unit.Implementation
{
    [TestFixture]
    public class HttpRequestInfoTest
    {
        [Test]
        public void TestCreate()
        {
            const string method = "method";
            const string path = "path";
            var query = new NameValueCollection();
            var headers = new NameValueCollection();
            var contentBytes = new byte[0];
            const string contentType = "contentType";

            var httpRequestInfo = HttpRequestInfo.Create(method, path, query, headers, contentBytes, contentType);

            httpRequestInfo.Method.ShouldBeEquivalentTo(method);
            httpRequestInfo.Path.ShouldBeEquivalentTo(path);
            httpRequestInfo.Query.ShouldBeEquivalentTo(query);
            httpRequestInfo.Headers.ShouldBeEquivalentTo(headers);
            httpRequestInfo.ContentBytes.ShouldBeEquivalentTo(contentBytes);
            httpRequestInfo.ContentType.ShouldBeEquivalentTo(contentType);
        }
    }
}