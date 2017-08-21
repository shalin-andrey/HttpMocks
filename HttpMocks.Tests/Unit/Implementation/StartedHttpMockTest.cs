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
        private Mock<IHandlingMockQueue> handlingMockQueue;

        public override void SetUp()
        {
            base.SetUp();

            httpListenerWrapper = NewMock<IHttpListenerWrapper>();
            handlingMockQueue = NewMock<IHandlingMockQueue>();
            startedHttpMock = new StartedHttpMock(httpListenerWrapper.Object, handlingMockQueue.Object);
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