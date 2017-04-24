using System;
using System.IO;
using System.Net;
using FluentAssertions;
using HttpMocks.Whens;
using NUnit.Framework;

namespace HttpMocks.Tests.Integrational
{
    [TestFixture]
    public class ContentPatternTest
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
            var postContentBytes = new byte[100];

            var httpMock = httpMocks.New("localhost");
            const string contentType = "application/text";
            httpMock
                .WhenRequestPost("/bills")
                .Content(postContentBytes, contentType)
                .ThenResponse(302);
            httpMock.Run();

            var url = BuildUrl(httpMock, "/bills");
            var response = Send(url, "POST", postContentBytes, contentType);

            response.StatusCode.ShouldBeEquivalentTo(302);
            response.ContentBytes.Length.ShouldBeEquivalentTo(0);

            httpMocks.Invoking(m => m.VerifyAll());
        }

        private static Uri BuildUrl(IHttpMock httpMock, string path)
        {
            return new UriBuilder(httpMock.MockUri.Scheme, httpMock.MockUri.Host, httpMock.MockUri.Port, path).Uri;
        }

        private static TestResponse Send(Uri url, string method, byte[] contentBytes, string contentType)
        {
            try
            {
                var request = WebRequest.Create(url);
                request.Method = method;
                request.Timeout = 2000;

                if (contentBytes.Length > 0)
                {
                    using (var stream = request.GetRequestStream())
                    {
                        stream.Write(contentBytes, 0, contentBytes.Length);
                    }
                    request.ContentType = contentType;
                }

                var httpWebResponse = (HttpWebResponse)request.GetResponse();
                return Convert(httpWebResponse);
            }
            catch (WebException e)
            {
                if (e.Response != null)
                {
                    return Convert((HttpWebResponse)e.Response);
                }

                return TestResponse.Create(452);
            }
        }

        private static TestResponse Convert(HttpWebResponse httpWebResponse)
        {
            return TestResponse.Create((int)httpWebResponse.StatusCode, ReadResponseContentBytes(httpWebResponse));
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
                stream?.CopyTo(mem, (int)httpWebResponse.ContentLength);
                return mem.ToArray();
            }
        }
    }
}