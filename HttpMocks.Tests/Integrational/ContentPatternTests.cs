using FluentAssertions;
using HttpMocks.Whens;
using NUnit.Framework;

namespace HttpMocks.Tests.Integrational
{
    [TestFixture]
    public class ContentPatternTests : IntegrationalTestsBase
    {
        [Test]
        public void TestSuccessThenGetReturn302()
        {
            var postContentBytes = new byte[100];
            const string contentType = "application/text";

            using (var httpMock = HttpMocks.New(DefaultMockUrl))
            {
                httpMock
                    .WhenRequestPost("/bills")
                    .Content(postContentBytes, contentType)
                    .ThenResponse(302);
            }

            var url = BuildUrl(DefaultMockUrl, "/bills");
            var response = Send(url, "POST", postContentBytes, contentType);

            response.StatusCode.ShouldBeEquivalentTo(302);
            response.ContentBytes.Length.ShouldBeEquivalentTo(0);
        }
    }
}