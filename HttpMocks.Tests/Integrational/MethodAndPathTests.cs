using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using HttpMocks.Exceptions;
using HttpMocks.Implementation;
using HttpMocks.Whens;
using NUnit.Framework;

namespace HttpMocks.Tests.Integrational
{
    [TestFixture]
    public class MethodAndPathTests : IntegrationalTestsBase
    {
        [TearDown]
        public override void TearDown()
        {
        }

        [Test]
        public void TestSuccessWhenGetReturn302()
        {
            using (var httpMock = HttpMocks.New(DefaultMockUrl))
            {
                httpMock
                    .WhenRequestGet("/bills")
                    .ThenResponse(302);
            }

            var url = BuildUrl(DefaultMockUrl, "/bills");
            var response = Send(url, "GET");

            response.StatusCode.ShouldBeEquivalentTo(302);
            response.ContentBytes.Length.ShouldBeEquivalentTo(0);

            HttpMocks.VerifyAll();
        }

        [Test]
        public void TestSuccessWhenHeadersDefined()
        {
            var headers = new NameValueCollection {{"X-Header-Name", "Header_Value"}};
            using (var httpMock = HttpMocks.New(DefaultMockUrl))
            {
                httpMock
                    .WhenRequestGet("/")
                    .Headers(headers)
                    .ThenResponse(200);
            }

            var url = BuildUrl(DefaultMockUrl, "/");
            var responseA = Send(url, "GET");

            responseA.StatusCode.ShouldBeEquivalentTo(500);

            var responseB = Send(url, "GET", headers: headers);

            responseB.StatusCode.ShouldBeEquivalentTo(200);

            HttpMocks.Invoking(m => m.VerifyAll())
                .ShouldThrowExactly<AssertHttpMockException>()
                .WithMessage("Actual request GET /, but not expected.");
        }

        [Test]
        public void TestSuccessWhenQueryDefined()
        {
            var query = new NameValueCollection {{"qp", "qv"}};
            using (var httpMock = HttpMocks.New(DefaultMockUrl))
            {
                httpMock
                    .WhenRequestGet("/")
                    .Query(query)
                    .ThenResponse(200);
            }

            var responseA = Send(BuildUrl(DefaultMockUrl, "/"), "GET");

            responseA.StatusCode.ShouldBeEquivalentTo(500);

            var responseB = Send(BuildUrl(DefaultMockUrl, "/", query), "GET");

            responseB.StatusCode.ShouldBeEquivalentTo(200);

            HttpMocks.Invoking(m => m.VerifyAll())
                .ShouldThrowExactly<AssertHttpMockException>()
                .WithMessage("Actual request GET /, but not expected.");
        }

        [Test]
        public void TestFailWhenActualRepeatMoreThatExpected()
        {
            using (var httpMock = HttpMocks.New(DefaultMockUrl))
            {
                httpMock
                    .WhenRequestGet("/bills")
                    .ThenResponse(200)
                    .Repeat(1);
            }

            var url = BuildUrl(DefaultMockUrl, "/bills");

            Send(url, "GET").StatusCode.ShouldBeEquivalentTo(200);

            Send(url, "GET").StatusCode.ShouldBeEquivalentTo(500);

            HttpMocks.Invoking(m => m.VerifyAll())
                .ShouldThrowExactly<AssertHttpMockException>()
                .WithMessage("Actual request GET /bills repeat count 2, but max expected repeat count 1.");
        }

        [Test]
        public void TestFailWhenDefaultActualRepeatMoreThatExpected()
        {
            using (var httpMock = HttpMocks.New(DefaultMockUrl))
            {
                httpMock
                    .WhenRequestGet("/bills")
                    .ThenResponse(200);
            }

            var url = BuildUrl(DefaultMockUrl, "/bills");

            Send(url, "GET").StatusCode.ShouldBeEquivalentTo(200);

            Send(url, "GET").StatusCode.ShouldBeEquivalentTo(500);

            HttpMocks.Invoking(m => m.VerifyAll())
                .ShouldThrowExactly<AssertHttpMockException>()
                .WithMessage("Actual request GET /bills repeat count 2, but max expected repeat count 1.");
        }

        [Test]
        public void TestFailWhenAnyActualRepeatMoreThatExpected()
        {
            using (var httpMock = HttpMocks.New(DefaultMockUrl))
            {
                httpMock
                    .WhenRequestGet("/bills")
                    .ThenResponse(200)
                    .RepeatAny();
            }

            var url = BuildUrl(DefaultMockUrl, "/bills");

            Send(url, "GET").StatusCode.ShouldBeEquivalentTo(200);

            Send(url, "GET").StatusCode.ShouldBeEquivalentTo(200);

            HttpMocks.VerifyAll();
        }

        [Test]
        public void TestSuccessWhenGetReturn200AndResult()
        {
            const string testDataString = "Test data";
            var content = Encoding.UTF8.GetBytes(testDataString);

            using (var httpMock = HttpMocks.New(DefaultMockUrl))
            {
                httpMock
                    .WhenRequestGet("/bills")
                    .ThenResponse(200)
                    .Content(content, "text/plain");
            }

            var url = BuildUrl(DefaultMockUrl, "/bills");
            var response = Send(url, "GET");

            response.StatusCode.ShouldBeEquivalentTo(200);
            response.ContentBytes.Length.ShouldBeEquivalentTo(content.Length);
            Encoding.UTF8.GetString(response.ContentBytes).ShouldBeEquivalentTo(testDataString);

            HttpMocks.VerifyAll();
        }

        [Test]
        public void TestFailWhenActualIsNotExpectedRequest()
        {
            using (var httpMock = HttpMocks.New(DefaultMockUrl))
            {
                httpMock
                    .WhenRequestGet("/bills/@guid")
                    .ThenResponse(302);
            }

            var url = BuildUrl(DefaultMockUrl, "/bills");
            var response = Send(url, "GET");

            response.StatusCode.ShouldBeEquivalentTo(500);
            response.ContentBytes.Length.ShouldBeEquivalentTo(0);

            HttpMocks
                .Invoking(m => m.VerifyAll())
                .ShouldThrowExactly<AssertHttpMockException>();
        }

        [Test]
        public void TestSuccessWhenResponseFromDelegate()
        {
            var paths = new List<string>();

            Func<HttpRequestInfo, HttpResponseInfo> processRequestInfo = request =>
            {
                paths.Add(request.Path);
                return HttpResponseInfo.Create(200);
            };

            using (var httpMock = HttpMocks.New(DefaultMockUrl))
            {
                httpMock
                    .WhenRequestGet("/bills/@guid")
                    .ThenResponse(i => processRequestInfo(i));
            }
            
            var guid = Guid.NewGuid();
            var url = BuildUrl(DefaultMockUrl, $"/bills/{guid}");
            var response = Send(url, "GET");

            response.StatusCode.ShouldBeEquivalentTo(200);
            response.ContentBytes.Length.ShouldBeEquivalentTo(0);

            paths.Count.ShouldBeEquivalentTo(1);
            paths[0].ShouldBeEquivalentTo($"/bills/{guid}");

            HttpMocks.VerifyAll();
        }

        [Test]
        public void TestSuccessWhenResponseFromAsyncDelegate()
        {
            var paths = new List<string>();

            Func<HttpRequestInfo, Task<HttpResponseInfo>> processRequestInfoAsync = request =>
            {
                paths.Add(request.Path);
                return Task.FromResult(HttpResponseInfo.Create(200));
            };

            using (var httpMock = HttpMocks.New(DefaultMockUrl))
            {
                httpMock
                    .WhenRequestGet("/bills/@guid")
                    .ThenResponse(i => processRequestInfoAsync(i));
            }

            var guid = Guid.NewGuid();
            var url = BuildUrl(DefaultMockUrl, $"/bills/{guid}");
            var response = Send(url, "GET");

            response.StatusCode.ShouldBeEquivalentTo(200);
            response.ContentBytes.Length.ShouldBeEquivalentTo(0);

            paths.Count.ShouldBeEquivalentTo(1);
            paths[0].ShouldBeEquivalentTo($"/bills/{guid}");

            HttpMocks.VerifyAll();
        }
    }
}