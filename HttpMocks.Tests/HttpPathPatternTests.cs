using FluentAssertions;
using NUnit.Framework;

namespace HttpMocks.Tests
{
    [TestFixture]
    public class HttpPathPatternTests
    {
        [Test]
        public void TestIsMatch()
        {
            var httpPathPattern = new HttpPathPattern("bills/@guid/payments");

            httpPathPattern.IsMatch("bills/eaf1d87d-d1fc-4dc6-9870-7aaff150031a/payments").Should().BeTrue();
            httpPathPattern.IsMatch(" /bills/eaf1d87d-d1fc-4dc6-9870-7aaff150031a/payments /").Should().BeTrue();

            httpPathPattern.IsMatch("bill/eaf1d87d-d1fc-4dc6-9870-7aaff150031a/payments").Should().BeFalse();
            httpPathPattern.IsMatch("bills/eaf1d87d-d1fc-4dc6-9870-7aaff150031a/payment").Should().BeFalse();
            httpPathPattern.IsMatch("bills/eaf1d87d-d1fc-4dc6-9870-7aaff150031a").Should().BeFalse();
            httpPathPattern.IsMatch("bills/eaftd87d-d1fc-4dc6-9870-7aaff150031a/payments").Should().BeFalse();
        }

        [Test]
        public void TestIsMatchWhenSimple()
        {
            var httpPathPattern = new HttpPathPattern("/billrows");

            httpPathPattern.IsMatch("/billrows").Should().BeTrue();
        }
    }
}