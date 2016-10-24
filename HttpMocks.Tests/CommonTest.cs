using System;
using System.Net;
using System.Threading;
using NUnit.Framework;

namespace HttpMocks.Tests
{
    [TestFixture]
    public class CommonTest
    {
        [Test]
        public void TestA()
        {
            var httpMock = HttpMock.New("http://localhost:23457/");

            httpMock
                .WhenRequestGet("/bills")
                .ThenResponse(302);

            var httpMockServer = new HttpMockRunner();
            httpMockServer.RunMock(httpMock);

            var request = HttpWebRequest.Create("http://localhost:23457/bills");
            request.Method = "GET";
            request.Timeout = 2000;
            var response = (HttpWebResponse) request.GetResponse();
            Console.WriteLine(response.StatusCode);

            httpMockServer.VerifyAll();
        }

        [Test]
        public void TestB()
        {
            var httpMock = HttpMock.New("http://localhost:23457/");

            httpMock
                .WhenRequestGet("/bills/@guid")
                .ThenResponse(302);

            var httpMockServer = new HttpMockRunner();
            httpMockServer.RunMock(httpMock);

            TrySend();

            httpMockServer.VerifyAll();
        }

        private static void TrySend()
        {
            try
            {
                var request = HttpWebRequest.Create("http://localhost:23457/bills");
                request.Method = "GET";
                request.Timeout = 2000;
                var response = (HttpWebResponse) request.GetResponse();
                Console.WriteLine(response.StatusCode);
            }
            catch (WebException e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}