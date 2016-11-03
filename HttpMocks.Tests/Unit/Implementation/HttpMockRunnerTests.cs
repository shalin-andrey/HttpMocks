using System;
using HttpMocks.Implementation;
using Moq;
using NUnit.Framework;

namespace HttpMocks.Tests.Unit.Implementation
{
    public class HttpMockRunnerTests : UnitTest
    {
        private Mock<IHandlingMockQueueFactory> handlingMockQueueFactoryMock;
        private Mock<IStartedHttpMockFactory> startedHttpMockFactoryMock;
        private HttpMockRunner httpMockRunner;

        public override void SetUp()
        {
            base.SetUp();

            handlingMockQueueFactoryMock = new Mock<IHandlingMockQueueFactory>(MockBehavior.Strict);
            startedHttpMockFactoryMock = new Mock<IStartedHttpMockFactory>(MockBehavior.Strict);
            httpMockRunner = new HttpMockRunner(handlingMockQueueFactoryMock.Object, startedHttpMockFactoryMock.Object);
        }

        [Test]
        public void TestRunMocks()
        {
            var httpRequestMocks = new HttpRequestMock[0];
            var mockUrl = new Uri("http://localhost/");
            var handlingMockQueueMock = new Mock<IHandlingMockQueue>(MockBehavior.Strict);
            var startedHttpMock = new Mock<IStartedHttpMock>(MockBehavior.Strict);

            handlingMockQueueFactoryMock.Setup(x => x.Create(httpRequestMocks)).Returns(handlingMockQueueMock.Object);
            startedHttpMockFactoryMock.Setup(x => x.Create(mockUrl, handlingMockQueueMock.Object)).Returns(startedHttpMock.Object);
            startedHttpMock.Setup(x => x.Start());

            httpMockRunner.RunMocks(mockUrl, httpRequestMocks);
        }
    }
}