using FluentAssertions;
using HttpMocks.Exceptions;
using NUnit.Framework;

namespace HttpMocks.Tests.Integrational
{
    [TestFixture]
    public class MoreThatOneMockTest : IntegrationalTestsBase
    {
        public override void TearDown()
        {
        }

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

            HttpMocks.VerifyAll();
        }

        [Test]
        public void TestNotActualWhenRepeatCountMoreActualCount()
        {
            IHttpMock httpMock1;
            using (httpMock1 = HttpMocks.New("localhost", 3465))
            {
                httpMock1
                    .WhenRequestGet("/bills/1")
                    .ThenResponse(200)
                    .Repeat(2);
            }

            Send(BuildUrl(httpMock1.MockUri, "/bills/1"), "GET").StatusCode.ShouldBeEquivalentTo(200);

            HttpMocks.Invoking(x => x.VerifyAll()).ShouldThrow<AssertHttpMockException>();
        }

        [Test]
        public void TestNotActualWhenNotExpected()
        {
            IHttpMock httpMock1;
            using (httpMock1 = HttpMocks.New("localhost", 3465))
            {
                httpMock1
                    .WhenRequestGet("/bills/1")
                    .ThenResponse(200);

                httpMock1
                    .WhenRequestGet("/bills/2")
                    .ThenResponse(200);
            }

            Send(BuildUrl(httpMock1.MockUri, "/bills/1"), "GET").StatusCode.ShouldBeEquivalentTo(200);

            HttpMocks.Invoking(x => x.VerifyAll()).ShouldThrow<AssertHttpMockException>();
        }
    }
}