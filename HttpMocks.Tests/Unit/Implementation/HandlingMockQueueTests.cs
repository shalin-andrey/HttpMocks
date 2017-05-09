using FluentAssertions;
using HttpMocks.Implementation;
using HttpMocks.Thens;
using HttpMocks.Whens.RequestPatterns;
using Moq;
using NUnit.Framework;

namespace HttpMocks.Tests.Unit.Implementation
{
    public class HandlingMockQueueTests : UnitTest
    {
        private Mock<IHttpMockDebugLogger> httpMockDebugLogger;

        public override void SetUp()
        {
            base.SetUp();

            httpMockDebugLogger = NewMock<IHttpMockDebugLogger>(MockBehavior.Loose);
        }

        [Test]
        public void TestDequeueWhenEmpty()
        {
            var handlingMockQueue = new HandlingMockQueue(httpMockDebugLogger.Object);
            handlingMockQueue.Enqueue(new HttpRequestMock[0]);
            handlingMockQueue.Dequeue(CreateRequestInfo("get", "/")).Should().BeNull();
        }

        [Test]
        public void TestDequeueWhenOneMock()
        {
            var mocks = new[] {CreateMock("get")};
            var handlingMockQueue = new HandlingMockQueue(httpMockDebugLogger.Object);
            handlingMockQueue.Enqueue(mocks);

            var handlingInfo = handlingMockQueue.Dequeue(CreateRequestInfo("get", "/"));

            handlingInfo.Should().NotBeNull();
            handlingInfo.IsUsageCountValid().Should().BeTrue();
            handlingInfo.HasAttempts().Should().BeFalse();
            handlingInfo.UsageCount.ShouldBeEquivalentTo(1);

            handlingInfo = handlingMockQueue.Dequeue(CreateRequestInfo("get", "/"));

            handlingInfo.Should().NotBeNull();
            handlingInfo.IsUsageCountValid().Should().BeFalse();
            handlingInfo.HasAttempts().Should().BeFalse();
            handlingInfo.UsageCount.ShouldBeEquivalentTo(2);
        }

        [Test]
        public void TestDequeueWhenChainMocks()
        {
            var mocks = new[] {CreateMock("get"), CreateMock("get")};
            var handlingMockQueue = new HandlingMockQueue(httpMockDebugLogger.Object);
            handlingMockQueue.Enqueue(mocks);

            var handlingInfo = handlingMockQueue.Dequeue(CreateRequestInfo("get", "/"));

            handlingInfo.Should().NotBeNull();
            handlingInfo.IsUsageCountValid().Should().BeTrue();
            handlingInfo.HasAttempts().Should().BeFalse();
            handlingInfo.UsageCount.ShouldBeEquivalentTo(1);

            handlingInfo = handlingMockQueue.Dequeue(CreateRequestInfo("get", "/"));

            handlingInfo.Should().NotBeNull();
            handlingInfo.IsUsageCountValid().Should().BeTrue();
            handlingInfo.HasAttempts().Should().BeFalse();
            handlingInfo.UsageCount.ShouldBeEquivalentTo(1);
        }

        [Test]
        public void TestDequeueWhenChainMocksAndNotUsages()
        {
            var httpRequestMock = CreateMock("get");
            var mocks = new[] {CreateMock("get"), httpRequestMock};
            var handlingMockQueue = new HandlingMockQueue(httpMockDebugLogger.Object);
            handlingMockQueue.Enqueue(mocks);

            var handlingInfo = handlingMockQueue.Dequeue(CreateRequestInfo("get", "/"));

            handlingInfo.Should().NotBeNull();
            handlingInfo.IsUsageCountValid().Should().BeTrue();
            handlingInfo.HasAttempts().Should().BeFalse();
            handlingInfo.UsageCount.ShouldBeEquivalentTo(1);

            handlingInfo = handlingMockQueue.Dequeue(CreateRequestInfo("get", "/"));

            handlingInfo.Should().NotBeNull();
            handlingInfo.IsUsageCountValid().Should().BeTrue();
            handlingInfo.HasAttempts().Should().BeFalse();
            handlingInfo.UsageCount.ShouldBeEquivalentTo(1);

            handlingInfo = handlingMockQueue.Dequeue(CreateRequestInfo("get", "/"));

            handlingInfo.Should().NotBeNull();
            handlingInfo.IsUsageCountValid().Should().BeFalse();
            handlingInfo.HasAttempts().Should().BeFalse();
            handlingInfo.UsageCount.ShouldBeEquivalentTo(2);
            handlingInfo.ResponseMock.Should().Be(httpRequestMock.Response);
        }

        private static HttpRequest CreateRequestInfo(string methodPattern, string pathPattern)
        {
            return new HttpRequest
            {
                Method = methodPattern,
                Path = pathPattern
            };
        }

        private static HttpRequestMock CreateMock(string method)
        {
            return new HttpRequestMock
            {
                Method = MethodPattern.Standart(method),
                Path = PathPattern.Smart("/"),
                Query = QueryPattern.Any(),
                Content = ContentPattern.Any(),
                Headers = HeadersPattern.Any(),
                Response = new HttpResponseMock
                {
                    RepeatCount = 1
                }
            };
        }
    }
}