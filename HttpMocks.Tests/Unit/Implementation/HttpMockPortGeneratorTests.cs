using System;
using System.Collections.Generic;
using FluentAssertions;
using HttpMocks.Implementation;
using NUnit.Framework;

namespace HttpMocks.Tests.Unit.Implementation
{
    [TestFixture]
    public class HttpMockPortGeneratorTests
    {
        [Test]
        public void TestSuccessGenerate()
        {
            var httpMockPortGenerator = new HttpMockPortGenerator();
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
            var httpMockPortGenerator = new HttpMockPortGenerator(1, 1);
            var expectedPort = httpMockPortGenerator.GeneratePort();
            expectedPort.ShouldBeEquivalentTo(1);
        }

        [Test]
        public void TestFailGenerateWhenOnePort()
        {
            var httpMockPortGenerator = new HttpMockPortGenerator(1, 1);
            var expectedPort = httpMockPortGenerator.GeneratePort();
            expectedPort.ShouldBeEquivalentTo(1);

            httpMockPortGenerator.Invoking(g => g.GeneratePort()).ShouldThrow<Exception>();
        }
    }
}