using System;
using System.Threading.Tasks;
using FluentAssertions;
using HttpMocks.Exceptions;
using HttpMocks.Implementation;
using HttpMocks.Verifications;
using Moq;
using NUnit.Framework;

namespace HttpMocks.Tests.Unit.Implementation
{
    public class HttpMockRunnerTests : UnitTest
    {
        private Mock<IHandlingMockQueueFactory> handlingMockQueueFactoryMock;
        private Mock<IStartedHttpMockFactory> startedHttpMockFactoryMock;
        private HttpMockRunner httpMockRunner;
        private Uri mockUrl;
        private HttpRequestMock[] httpRequestMocks;
        private Mock<IHandlingMockQueue> handlingMockQueueMock;
        private Mock<IStartedHttpMock> startedHttpMock;

        public override void SetUp()
        {
            base.SetUp();

            mockUrl = new Uri("http://localhost/");

            handlingMockQueueFactoryMock = NewMock<IHandlingMockQueueFactory>();
            startedHttpMockFactoryMock = NewMock<IStartedHttpMockFactory>();
            httpMockRunner = new HttpMockRunner(handlingMockQueueFactoryMock.Object, startedHttpMockFactoryMock.Object);

            httpRequestMocks = new HttpRequestMock[0];
            handlingMockQueueMock = NewMock<IHandlingMockQueue>();
            startedHttpMock = NewMock<IStartedHttpMock>();

            handlingMockQueueFactoryMock.Setup(x => x.Create(httpRequestMocks)).Returns(handlingMockQueueMock.Object);
            startedHttpMockFactoryMock.Setup(x => x.Create(mockUrl, handlingMockQueueMock.Object)).Returns(startedHttpMock.Object);
            startedHttpMock.Setup(x => x.Start());
        }

        [Test]
        public void TestRunMocks()
        {
            httpMockRunner.RunMocks(mockUrl, httpRequestMocks);
        }

        [Test]
        public void TestVerifyAllWhenHasResults()
        {
            httpMockRunner.RunMocks(mockUrl, httpRequestMocks);

            var verificationResults = new[]
            {
                VerificationResult.Create("VerificationResult1")
            };
            startedHttpMock.Setup(x => x.StopAsync()).Returns(Task.FromResult(verificationResults));

            httpMockRunner
                .Invoking(x => x.VerifyAll())
                .ShouldThrow<AssertHttpMockException>()
                .Where(e => e.Message.Contains("VerificationResult1"));
        }

        [Test]
        public void TestVerifyAllWhenHasNotResults()
        {
           httpMockRunner.RunMocks(mockUrl, httpRequestMocks);

            startedHttpMock.Setup(x => x.StopAsync()).Returns(Task.FromResult(new VerificationResult[0]));

            httpMockRunner.VerifyAll();
        }
    }
}