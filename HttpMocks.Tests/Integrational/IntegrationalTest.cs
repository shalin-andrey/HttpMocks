using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
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
    public class IntegrationalTest
    {
        private HttpMockRepository httpMocks;

        [SetUp]
        public void SetUp()
        {
            httpMocks = new HttpMockRepository();
        }

        [Test]
        public void TestSuccessWhenGetReturn302()
        {
            var httpMock = httpMocks.New("localhost");
            httpMock
                .WhenRequestGet("/bills")
                .ThenResponse(302);
            httpMock.Run();

            var url = BuildUrl(httpMock, "/bills");
            var response = Send(url, "GET");

            response.StatusCode.ShouldBeEquivalentTo(302);
            response.ContentBytes.Length.ShouldBeEquivalentTo(0);

            httpMocks.VerifyAll();
        }

        [Test]
        public void TestSuccessWhenHeadersDefined()
        {
            var headers = new NameValueCollection {{"X-Header-Name", "Header_Value"}};
            var httpMock = httpMocks.New("localhost");
            httpMock
                .WhenRequestGet("/")
                .Headers(headers)
                .ThenResponse(200);
            httpMock.Run();

            var url = BuildUrl(httpMock, "/");
            var responseA = Send(url, "GET");

            responseA.StatusCode.ShouldBeEquivalentTo(500);

            var responseB = Send(url, "GET", headers);

            responseB.StatusCode.ShouldBeEquivalentTo(200);

            httpMocks.Invoking(m => m.VerifyAll())
                .ShouldThrowExactly<AssertHttpMockException>()
                .WithMessage("Actual request GET /, but not expected.");
        }

        [Test]
        public void TestSuccessWhenQueryDefined()
        {
            var query = new NameValueCollection {{"qp", "qv"}};
            var httpMock = httpMocks.New("localhost");
            httpMock
                .WhenRequestGet("/")
                .Query(query)
                .ThenResponse(200);
            httpMock.Run();

            var responseA = Send(BuildUrl(httpMock, "/"), "GET");

            responseA.StatusCode.ShouldBeEquivalentTo(500);

            var responseB = Send(BuildUrl(httpMock, "/", query), "GET");

            responseB.StatusCode.ShouldBeEquivalentTo(200);

            httpMocks.Invoking(m => m.VerifyAll())
                .ShouldThrowExactly<AssertHttpMockException>()
                .WithMessage("Actual request GET /, but not expected.");
        }

        [Test]
        public void TestFailWhenActualRepeatMoreThatExpected()
        {
            var httpMock = httpMocks.New("localhost");
            httpMock
                .WhenRequestGet("/bills")
                .ThenResponse(200)
                .Repeat(1);
            httpMock.Run();

            var url = BuildUrl(httpMock, "/bills");

            Send(url, "GET").StatusCode.ShouldBeEquivalentTo(200);

            Send(url, "GET").StatusCode.ShouldBeEquivalentTo(500);

            httpMocks.Invoking(m => m.VerifyAll())
                .ShouldThrowExactly<AssertHttpMockException>()
                .WithMessage("Actual request GET /bills repeat count 2, but max expected repeat count 1.");
        }

        [Test]
        public void TestFailWhenDefaultActualRepeatMoreThatExpected()
        {
            var httpMock = httpMocks.New("localhost");
            httpMock
                .WhenRequestGet("/bills")
                .ThenResponse(200);
            httpMock.Run();

            var url = BuildUrl(httpMock, "/bills");

            Send(url, "GET").StatusCode.ShouldBeEquivalentTo(200);

            Send(url, "GET").StatusCode.ShouldBeEquivalentTo(500);

            httpMocks.Invoking(m => m.VerifyAll())
                .ShouldThrowExactly<AssertHttpMockException>()
                .WithMessage("Actual request GET /bills repeat count 2, but max expected repeat count 1.");
        }

        [Test]
        public void TestFailWhenAnyActualRepeatMoreThatExpected()
        {
            var httpMock = httpMocks.New("localhost");
            httpMock
                .WhenRequestGet("/bills")
                .ThenResponse(200)
                .RepeatAny();
            httpMock.Run();

            var url = BuildUrl(httpMock, "/bills");

            Send(url, "GET").StatusCode.ShouldBeEquivalentTo(200);

            Send(url, "GET").StatusCode.ShouldBeEquivalentTo(200);

            httpMocks.VerifyAll();
        }

        [Test]
        public void TestSuccessWhenGetReturn200AndResult()
        {
            const string testDataString = "Test data";
            var content = Encoding.UTF8.GetBytes(testDataString);

            var httpMock = httpMocks.New("localhost");
            httpMock
                .WhenRequestGet("/bills")
                .ThenResponse(200)
                .Content(content, "text/plain");
            httpMock.Run();

            var url = BuildUrl(httpMock, "/bills");
            var response = Send(url, "GET");

            httpMocks.VerifyAll();

            response.StatusCode.ShouldBeEquivalentTo(200);
            response.ContentBytes.Length.ShouldBeEquivalentTo(content.Length);
            Encoding.UTF8.GetString(response.ContentBytes).ShouldBeEquivalentTo(testDataString);
        }

        [Test]
        public void TestFailWhenActualIsNotExpectedRequest()
        {
            var httpMock = httpMocks.New("localhost");
            httpMock
                .WhenRequestGet("/bills/@guid")
                .ThenResponse(302);
            httpMock.Run();

            var url = BuildUrl(httpMock, "/bills");
            var response = Send(url, "GET");

            response.StatusCode.ShouldBeEquivalentTo(500);
            response.ContentBytes.Length.ShouldBeEquivalentTo(0);

            httpMocks
                .Invoking(m => m.VerifyAll())
                .ShouldThrowExactly<AssertHttpMockException>();
        }

        [Test]
        public void TestSuccessWhenResponseFromDelegate()
        {
            var paths = new List<string>();
            var httpMock = httpMocks.New("localhost");
            httpMock
                .WhenRequestGet("/bills/@guid")
                .ThenResponse(i => ProcessRequestInfo(i, paths));
            httpMock.Run();

            var guid = Guid.NewGuid();
            var url = BuildUrl(httpMock, $"/bills/{guid}");
            var response = Send(url, "GET");

            response.StatusCode.ShouldBeEquivalentTo(200);
            response.ContentBytes.Length.ShouldBeEquivalentTo(0);

            paths.Count.ShouldBeEquivalentTo(1);
            paths[0].ShouldBeEquivalentTo($"/bills/{guid}");
        }

        [Test]
        public void TestSuccessWhenResponseFromAsyncDelegate()
        {
            var paths = new List<string>();
            var httpMock = httpMocks.New("localhost");
            httpMock
                .WhenRequestGet("/bills/@guid")
                .ThenResponse(i => ProcessRequestInfoAsync(i, paths));
            httpMock.Run();

            var guid = Guid.NewGuid();
            var url = BuildUrl(httpMock, $"/bills/{guid}");
            var response = Send(url, "GET");

            response.StatusCode.ShouldBeEquivalentTo(200);
            response.ContentBytes.Length.ShouldBeEquivalentTo(0);

            paths.Count.ShouldBeEquivalentTo(1);
            paths[0].ShouldBeEquivalentTo($"/bills/{guid}");
        }

        private static HttpResponseInfo ProcessRequestInfo(HttpRequestInfo requestInfo, List<string> paths)
        {
            paths.Add(requestInfo.Path);
            return HttpResponseInfo.Create(200);
        }

        private static Task<HttpResponseInfo> ProcessRequestInfoAsync(HttpRequestInfo requestInfo, List<string> paths)
        {
            paths.Add(requestInfo.Path);
            return Task.FromResult(HttpResponseInfo.Create(200));
        }

        private static Uri BuildUrl(IHttpMock httpMock, string path, NameValueCollection query = null)
        {
            var uriBuilder = new UriBuilder(httpMock.MockUri.Scheme, httpMock.MockUri.Host, httpMock.MockUri.Port, path);

            if (query != null)
            {
                uriBuilder.Query = string.Join("&", query.AllKeys.Select(x => $"{x}={query[x]}"));
            }

            return uriBuilder.Uri;
        }

        private static TestResponse Send(Uri url, string method, NameValueCollection headers = null)
        {
            try
            {
                var request = WebRequest.Create(url);
                request.Method = method;

                if (headers != null)
                {
                    foreach (var headerName in headers.AllKeys)
                    {
                        request.Headers.Add(headerName, headers[headerName]);
                    }
                }

                request.Timeout = 2000;

                var httpWebResponse = (HttpWebResponse) request.GetResponse();
                return Convert(httpWebResponse);
            }
            catch (WebException e)
            {
                if (e.Response != null)
                {
                    return Convert((HttpWebResponse) e.Response);
                }

                return TestResponse.Create(452);
            }
        }

        private static TestResponse Convert(HttpWebResponse httpWebResponse)
        {
            return TestResponse.Create((int) httpWebResponse.StatusCode, ReadResponseContentBytes(httpWebResponse));
        }

        private static byte[] ReadResponseContentBytes(HttpWebResponse httpWebResponse)
        {
            if (httpWebResponse.ContentLength <= 0)
            {
                return new byte[0];
            }

            var mem = new MemoryStream();
            using (var stream = httpWebResponse.GetResponseStream())
            {
                stream?.CopyTo(mem, (int) httpWebResponse.ContentLength);
                return mem.ToArray();
            }
        }
    }
}