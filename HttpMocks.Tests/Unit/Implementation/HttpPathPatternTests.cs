using System.Collections.Generic;
using FluentAssertions;
using HttpMocks.Whens.RequestPatterns;
using HttpMocks.Whens.RequestPatterns.PathPatterns;
using NUnit.Framework;

namespace HttpMocks.Tests.Unit.Implementation
{
    [TestFixture]
    public class HttpPathPatternTests
    {
        [Test]
        [TestCaseSource(nameof(GenerateTestCaseDatas))]
        public void TestIsMatchWhenUrlContainsGuid(string pattern, string value, bool expected)
        {
            IHttpRequestPathPattern httpPathPattern = new SmartPathPattern(pattern);
            httpPathPattern.IsMatch(value).Should().Be(expected);
        }

        private static IEnumerable<TestCaseData> GenerateTestCaseDatas()
        {
            yield return new TestCaseData("bills/@guid/payments", "bills/eaf1d87d-d1fc-4dc6-9870-7aaff150031a/payments", true);
            yield return new TestCaseData("bills/@guid/payments", " /bills/eaf1d87d-d1fc-4dc6-9870-7aaff150031a/payments/", true);
            yield return new TestCaseData("bills/@guid/payments", "bill/eaf1d87d-d1fc-4dc6-9870-7aaff150031a/payments", false);
            yield return new TestCaseData("bills/@guid/payments", "bills/eaf1d87d-d1fc-4dc6-9870-7aaff150031a/payment", false);
            yield return new TestCaseData("bills/@guid/payments", "bills/eaf1d87d-d1fc-4dc6-9870-7aaff150031a", false);
            yield return new TestCaseData("bills/@guid/payments", "bills/eaftd87d-d1fc-4dc6-9870-7aaff150031a/payments", false);
            yield return new TestCaseData("bills/@guid/payments/@guid", "/bills/eaf1d87d-d1fc-4dc6-9870-7aaff150031a/payments/88a4932c-c20f-4c2e-8787-af616f5f4124", true);
            yield return new TestCaseData("/billrows", "/billrows", true);
        }
    }
}