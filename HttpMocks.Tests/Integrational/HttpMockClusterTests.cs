using FluentAssertions;
using NUnit.Framework;

namespace HttpMocks.Tests.Integrational
{
    [TestFixture]
    public class HttpMockClusterTests : IntegrationalTestsBase
    {
        [Test]
        public void TestSuccess()
        {
            IHttpMock httpMock1;
            using (httpMock1 = HttpMocks.NewCluster("localhost", 2))
            {
                httpMock1
                    .WhenRequestGet("/bills/1")
                    .ThenResponse(200);
            }

            Send(BuildUrl(httpMock1.MockUri, "/bills/1"), "GET").StatusCode.ShouldBeEquivalentTo(200);
        }
    }
}