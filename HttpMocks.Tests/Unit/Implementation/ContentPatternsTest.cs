using System.Text;
using FluentAssertions;
using HttpMocks.Whens.RequestPatterns;
using HttpMocks.Whens.RequestPatterns.ContentPatterns;
using NUnit.Framework;

namespace HttpMocks.Tests.Unit.Implementation
{
    public class ContentPatternsTest : UnitTest
    {
        [Test]
        public void TestAny()
        {
            IHttpRequestContentPattern contentPattern = ContentPattern.Any();
            contentPattern.IsMatch(GenBytes(), GenString()).ShouldBeEquivalentTo(true);
        }

        [Test]
        public void TestBinary()
        {
            var contentBytes = GenBytes();
            var contentType = GenString();

            Check(ContentPattern.Binary(contentBytes, contentType), contentBytes, contentType);
            CheckWithoutContentType(ContentPattern.Binary(contentBytes), contentBytes, contentType);
        }

        [Test]
        public void TestBase64()
        {
            var contentBase64 = GenBase64String();
            var contentType = GenString();

            var rightContentBytes = Encoding.ASCII.GetBytes(contentBase64);

            Check(ContentPattern.Base64(contentBase64, contentType), rightContentBytes, contentType);
            CheckWithoutContentType(ContentPattern.Base64(contentBase64), rightContentBytes, contentType);
        }

        private void Check(IHttpRequestContentPattern contentPattern, byte[] rightContentBytes, string rightContentType)
        {
            contentPattern.IsMatch(rightContentBytes, rightContentType).ShouldBeEquivalentTo(true);
            contentPattern.IsMatch(GenBytes(), rightContentType).ShouldBeEquivalentTo(false);
            contentPattern.IsMatch(rightContentBytes, GenString()).ShouldBeEquivalentTo(false);
            contentPattern.IsMatch(GenBytes(), GenString()).ShouldBeEquivalentTo(false);
        }

        private void CheckWithoutContentType(IHttpRequestContentPattern contentPattern, byte[] rightContentBytes, string rightContentType)
        {
            contentPattern.IsMatch(rightContentBytes, rightContentType).ShouldBeEquivalentTo(true);
            contentPattern.IsMatch(GenBytes(), rightContentType).ShouldBeEquivalentTo(false);
            contentPattern.IsMatch(rightContentBytes, GenString()).ShouldBeEquivalentTo(true);
            contentPattern.IsMatch(GenBytes(), GenString()).ShouldBeEquivalentTo(false);
        }
    }
}