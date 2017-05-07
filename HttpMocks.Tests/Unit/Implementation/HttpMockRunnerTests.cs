using System.Threading.Tasks;
using FluentAssertions;
using HttpMocks.Exceptions;
using HttpMocks.Implementation;
using HttpMocks.Implementation.Core;
using HttpMocks.Verifications;
using Moq;
using NUnit.Framework;

namespace HttpMocks.Tests.Unit.Implementation
{
    public class HttpMockRunnerTests : UnitTest
    {
        private Mock<IStartedHttpMockFactory> startedHttpMockFactory;
        private HttpMockRunner httpMockRunner;
        private Mock<IHttpListenerWrapperFactory> httpListenerWrapperFactory;

        public override void SetUp()
        {
            base.SetUp();

            startedHttpMockFactory = NewMock<IStartedHttpMockFactory>();
            httpListenerWrapperFactory = NewMock<IHttpListenerWrapperFactory>();
            httpMockRunner = new HttpMockRunner(startedHttpMockFactory.Object, httpListenerWrapperFactory.Object);
        }

        [Test]
        public void TestRunMocks()
        {
            var mockUrlEnumerator = NewMock<IMockUrlEnumerator>();
            var httpListenerWrapper = NewMock<IHttpListenerWrapper>();
            var startedHttpMock = NewMock<IStartedHttpMock>();

            httpListenerWrapperFactory.Setup(x => x.CreateAndStart(mockUrlEnumerator.Object)).Returns(httpListenerWrapper.Object);
            startedHttpMockFactory.Setup(x => x.Create(httpListenerWrapper.Object)).Returns(startedHttpMock.Object);

            var actual = httpMockRunner.RunMocks(mockUrlEnumerator.Object);

            actual.ShouldBeEquivalentTo(startedHttpMock.Object);
        }

        [Test]
        public void TestVerifyAllWhenHasResults()
        {
            var mockUrlEnumerator = NewMock<IMockUrlEnumerator>();
            var httpListenerWrapper = NewMock<IHttpListenerWrapper>();
            var startedHttpMock = NewMock<IStartedHttpMock>();

            httpListenerWrapperFactory.Setup(x => x.CreateAndStart(mockUrlEnumerator.Object)).Returns(httpListenerWrapper.Object);
            startedHttpMockFactory.Setup(x => x.Create(httpListenerWrapper.Object)).Returns(startedHttpMock.Object);

            httpMockRunner.RunMocks(mockUrlEnumerator.Object);

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
            var mockUrlEnumerator = NewMock<IMockUrlEnumerator>();
            var httpListenerWrapper = NewMock<IHttpListenerWrapper>();
            var startedHttpMock = NewMock<IStartedHttpMock>();

            httpListenerWrapperFactory.Setup(x => x.CreateAndStart(mockUrlEnumerator.Object)).Returns(httpListenerWrapper.Object);
            startedHttpMockFactory.Setup(x => x.Create(httpListenerWrapper.Object)).Returns(startedHttpMock.Object);

            httpMockRunner.RunMocks(mockUrlEnumerator.Object);
            
            startedHttpMock.Setup(x => x.StopAsync()).Returns(Task.FromResult(new VerificationResult[0]));

            httpMockRunner.VerifyAll();
        }
    }
}