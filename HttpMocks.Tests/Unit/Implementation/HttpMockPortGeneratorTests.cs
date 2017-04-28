using System;
using System.Collections.Generic;
using FluentAssertions;
using HttpMocks.Implementation;
using Moq;
using NUnit.Framework;

namespace HttpMocks.Tests.Unit.Implementation
{
    [TestFixture]
    public class HttpMockPortGeneratorTests : UnitTest
    {
        private Mock<IUnavailablePortsProvider> unanavailablePortsProvider;

        [SetUp]
        public void SetUp()
        {
            unanavailablePortsProvider = NewMock<IUnavailablePortsProvider>();
            unanavailablePortsProvider.Setup(x => x.GetUnavailablePorts()).Returns(new int[0]);
        }

        [Test]
        public void TestSuccessGenerate()
        {
            var httpMockPortGenerator = new HttpMockPortGenerator(unanavailablePortsProvider.Object);
            var hashSet = new HashSet<int>();
            for (var i = 0; i < 10000; i++)
            {
                var port = httpMockPortGenerator.GeneratePort();
                hashSet.Contains(port).ShouldBeEquivalentTo(false);
                hashSet.Add(port);
            }
        }

        [Test]
        public void TestSuccessGenerateWhenOnePort()
        {
            var httpMockPortGenerator = new HttpMockPortGenerator(unanavailablePortsProvider.Object, 1, 1);
            var expectedPort = httpMockPortGenerator.GeneratePort();
            expectedPort.ShouldBeEquivalentTo(1);
        }

        [Test]
        public void TestFailGenerateWhenOnePort()
        {
            var httpMockPortGenerator = new HttpMockPortGenerator(unanavailablePortsProvider.Object, 1, 1);
            var expectedPort = httpMockPortGenerator.GeneratePort();
            expectedPort.ShouldBeEquivalentTo(1);

            httpMockPortGenerator.Invoking(g => g.GeneratePort()).ShouldThrow<Exception>();
        }
    }
}