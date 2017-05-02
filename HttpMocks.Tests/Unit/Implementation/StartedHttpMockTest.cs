using System;
using FluentAssertions;
using HttpMocks.Exceptions;
using HttpMocks.Implementation;
using HttpMocks.Implementation.Core;
using Moq;
using NUnit.Framework;

namespace HttpMocks.Tests.Unit.Implementation
{
    public class StartedHttpMockTest : UnitTest
    {
        private Mock<IHandlingMockQueue> handlingMockQueue;
        private Mock<IHttpMocksExceptionFactory> httpMocksExceptionFactory;
        private StartedHttpMock startedHttpMock;
        private Mock<IHttpListenerWrapper> httpListenerWrapper;

        public override void SetUp()
        {
            base.SetUp();

            handlingMockQueue = NewMock<IHandlingMockQueue>();
            httpMocksExceptionFactory = NewMock<IHttpMocksExceptionFactory>();
            httpListenerWrapper = NewMock<IHttpListenerWrapper>();
            startedHttpMock = new StartedHttpMock(httpListenerWrapper.Object, handlingMockQueue.Object, httpMocksExceptionFactory.Object);
        }

        [Test]
        public void TestStartWhenFailStartHttpListener()
        {
            var mockUrl = new UriBuilder(@"http://localhost:345/path").Uri;
            var exception = new Exception();

            httpListenerWrapper.Setup(x => x.Prefix).Returns(mockUrl);
            httpListenerWrapper.Setup(x => x.Start()).Throws<Exception>();
            httpMocksExceptionFactory
                .Setup(x => x.CreateWithDiagnostic(mockUrl, "Can't start http listener", It.IsAny<Exception>()))
                .Returns(exception);

            startedHttpMock
                .Invoking(x => x.Start())
                .ShouldThrow<Exception>()
                .Where(x => x == exception);
        }
    }
}