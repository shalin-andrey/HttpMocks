using System;
using System.IO;
using System.Net;
using System.Text;
using FluentAssertions;
using HttpMocks.Verifications;
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
        public void TestSuccessThenGetReturn302()
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

            httpMocks.Invoking(m => m.VerifyAll());
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

            httpMocks.Invoking(m => m.VerifyAll());
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
                .ThenContent(content, "text/plain");
            httpMock.Run();

            var url = BuildUrl(httpMock, "/bills");
            var response = Send(url, "GET");

            httpMocks.Invoking(m => m.VerifyAll());

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

            httpMocks.Invoking(m => m.VerifyAll()).ShouldThrowExactly<AssertHttpMockException>();
        }

        private static Uri BuildUrl(IHttpMock httpMock, string path)
        {
            return new UriBuilder(httpMock.MockUri.Scheme, httpMock.MockUri.Host, httpMock.MockUri.Port, path).Uri;
        }

        private static TestResponse Send(Uri url, string method)
        {
            try
            {
                var request = WebRequest.Create(url);
                request.Method = method;

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