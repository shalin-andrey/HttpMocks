using FluentAssertions;
using NUnit.Framework;

namespace HttpMocks.Tests.Integrational
{
    [TestFixture]
    public class MoreThatOneMockTest : IntegrationalTestsBase
    {
        [Test]
        public void TestSuccess()
        {
            IHttpMock httpMock1;
            using (httpMock1 = HttpMocks.New("localhost", 3465))
            {
                httpMock1
                    .WhenRequestGet("/bills/1")
                    .ThenResponse(200);
            }

            IHttpMock httpMock2;
            using (httpMock2 = HttpMocks.New("localhost", 3466))
            {
                httpMock2
                    .WhenRequestGet("/bills/2")
                    .ThenResponse(200);
            }

            Send(BuildUrl(httpMock1.MockUri, "/bills/1"), "GET").StatusCode.ShouldBeEquivalentTo(200);
            Send(BuildUrl(httpMock2.MockUri, "/bills/2"), "GET").StatusCode.ShouldBeEquivalentTo(200);
        }
    }
}