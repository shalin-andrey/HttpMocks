using System;
using FluentAssertions;
using HttpMocks.Implementation;
using HttpMocks.Implementation.Core;
using Moq;
using NUnit.Framework;

namespace HttpMocks.Tests.Unit.Implementation
{
    public class StartedHttpMockTest : UnitTest
    {
        private StartedHttpMock startedHttpMock;
        private Mock<IHttpListenerWrapper> httpListenerWrapper;
        private Mock<IHttpMockDebugLogger> httpMockDebugLogger;

        public override void SetUp()
        {
            base.SetUp();

            httpListenerWrapper = NewMock<IHttpListenerWrapper>();
            httpMockDebugLogger = NewMock<IHttpMockDebugLogger>(MockBehavior.Loose);
            startedHttpMock = new StartedHttpMock(httpListenerWrapper.Object, httpMockDebugLogger.Object);
        }

        [Test]
        public void TestMockUrl()
        {
            var mockUrl = new Uri(@"http://localhost:4389/");

            httpListenerWrapper.Setup(x => x.MockUrl).Returns(mockUrl);
            
            startedHttpMock.MockUrl.ShouldBeEquivalentTo(mockUrl);
        }
    }
}