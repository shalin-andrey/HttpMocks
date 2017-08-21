using System;
using FluentAssertions;
using HttpMocks.Implementation;
using HttpMocks.Implementation.Core;
using Moq;
using NUnit.Framework;

namespace HttpMocks.Tests.Unit.Implementation
{
    public class HttpMockRunnerTests : UnitTest
    {
        private Mock<IStartedHttpMockFactory> startedHttpMockFactory;
        private HttpMockRunner httpMockRunner;
        private Mock<IHttpListenerWrapperFactory> httpListenerWrapperFactory;
        private Mock<IHttpMockDebugLoggerFactory> httpMockDebugLoggerFactory;
        private Mock<IHandlingMockQueue> handlingMockQueue;

        public override void SetUp()
        {
            base.SetUp();

            startedHttpMockFactory = NewMock<IStartedHttpMockFactory>();
            handlingMockQueue = NewMock<IHandlingMockQueue>();
            httpListenerWrapperFactory = NewMock<IHttpListenerWrapperFactory>();
            httpMockDebugLoggerFactory = NewMock<IHttpMockDebugLoggerFactory>(MockBehavior.Loose);
            httpMockRunner = new HttpMockRunner(startedHttpMockFactory.Object, httpListenerWrapperFactory.Object);
        }

        [Test]
        public void TestRunMocks()
        {
            var mockUrlEnumerator = NewMock<IMockUrlEnumerator>();
            var httpListenerWrapper = NewMock<IHttpListenerWrapper>();
            var startedHttpMock = NewMock<IStartedHttpMock>();
            var httpMockDebugLogger = NewMock<IHttpMockDebugLogger>(MockBehavior.Loose);
            var mockUrl = new Uri("http://localhost:80/");

            httpListenerWrapper.Setup(x => x.MockUrl).Returns(mockUrl);
            httpListenerWrapperFactory.Setup(x => x.CreateAndStart(mockUrlEnumerator.Object)).Returns(httpListenerWrapper.Object);
            httpMockDebugLoggerFactory.Setup(x => x.Create(mockUrl)).Returns(httpMockDebugLogger.Object);
            startedHttpMockFactory.Setup(x => x.Create(httpListenerWrapper.Object, handlingMockQueue.Object)).Returns(startedHttpMock.Object);

            var actual = httpMockRunner.RunMocks(mockUrlEnumerator.Object, handlingMockQueue.Object);

            actual.ShouldBeEquivalentTo(startedHttpMock.Object);
        }
    }
}