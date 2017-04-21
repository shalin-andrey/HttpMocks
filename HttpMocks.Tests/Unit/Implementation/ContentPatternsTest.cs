using System.Text;
using FluentAssertions;
using HttpMocks.Whens;
using HttpMocks.Whens.HttpRequestMockContentPatterns;
using NUnit.Framework;

namespace HttpMocks.Tests.Unit.Implementation
{
    public class ContentPatternsTest : UnitTest
    {
        [Test]
        public void TestAny()
        {
            var contentPattern = ContentPatterns.Any();
            contentPattern.IsMatch(GenBytes(), GenString()).ShouldBeEquivalentTo(true);
        }

        [Test]
        public void TestBinary()
        {
            var contentBytes = GenBytes();
            var contentType = GenString();

            Check(ContentPatterns.Binary(contentBytes, contentType), contentBytes, contentType);
            CheckWithoutContentType(ContentPatterns.Binary(contentBytes), contentBytes, contentType);
        }

        [Test]
        public void TestBase64()
        {
            var contentBase64 = GenBase64String();
            var contentType = GenString();

            var rightContentBytes = Encoding.ASCII.GetBytes(contentBase64);

            Check(ContentPatterns.Base64(contentBase64, contentType), rightContentBytes, contentType);
            CheckWithoutContentType(ContentPatterns.Base64(contentBase64), rightContentBytes, contentType);
        }

        private void Check(IHttpRequestMockContentPattern contentPattern, byte[] rightContentBytes, string rightContentType)
        {
            contentPattern.IsMatch(rightContentBytes, rightContentType).ShouldBeEquivalentTo(true);
            contentPattern.IsMatch(GenBytes(), rightContentType).ShouldBeEquivalentTo(false);
            contentPattern.IsMatch(rightContentBytes, GenString()).ShouldBeEquivalentTo(false);
            contentPattern.IsMatch(GenBytes(), GenString()).ShouldBeEquivalentTo(false);
        }

        private void CheckWithoutContentType(IHttpRequestMockContentPattern contentPattern, byte[] rightContentBytes, string rightContentType)
        {
            contentPattern.IsMatch(rightContentBytes, rightContentType).ShouldBeEquivalentTo(true);
            contentPattern.IsMatch(GenBytes(), rightContentType).ShouldBeEquivalentTo(false);
            contentPattern.IsMatch(rightContentBytes, GenString()).ShouldBeEquivalentTo(true);
            contentPattern.IsMatch(GenBytes(), GenString()).ShouldBeEquivalentTo(false);
        }
    }
}