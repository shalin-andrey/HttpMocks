using System;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using FluentAssertions;
using NUnit.Framework;

namespace HttpMocks.Tests.Integrational
{
    [TestFixture]
    public class MoreThatOneMockTest
    {
        private HttpMockRepository httpMocks;

        [SetUp]
        public void SetUp()
        {
            httpMocks = new HttpMockRepository();
        }

        [Test]
        public void TestSuccess()
        {
            IHttpMock httpMock1;
            using (httpMock1 = httpMocks.New("localhost"))
            {
                httpMock1
                    .WhenRequestGet("/bills/1")
                    .ThenResponse(200);
            }

            IHttpMock httpMock2;
            using (httpMock2 = httpMocks.New("localhost"))
            {
                httpMock2
                    .WhenRequestGet("/bills/2")
                    .ThenResponse(200);
            }

            Send(BuildUrl(httpMock1, "/bills/1"), "GET").StatusCode.ShouldBeEquivalentTo(200);
            Send(BuildUrl(httpMock2, "/bills/2"), "GET").StatusCode.ShouldBeEquivalentTo(200);
        }

        [TearDown]
        public void TearDown()
        {
            httpMocks.VerifyAll();
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