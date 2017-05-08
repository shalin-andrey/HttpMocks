using System;
using System.Linq;
using Moq;
using NUnit.Framework;

namespace HttpMocks.Tests.Unit.Implementation
{
    [TestFixture]
    public abstract class UnitTest
    {
        private readonly Random random = new Random(DateTime.Now.Millisecond);
        private readonly string letters = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM";

        [SetUp]
        public virtual void SetUp()
        {
        }

        protected static Mock<T> NewMock<T>(MockBehavior mockBehavior = MockBehavior.Strict) where T : class 
        {
            return new Mock<T>(mockBehavior);
        }

        protected string GenBase64String(int binaryLength = 100)
        {
            var bytes = new byte[binaryLength];
            random.NextBytes(bytes);
            return Convert.ToBase64String(bytes);
        }

        protected byte[] GenBytes(int length = 100)
        {
            var bytes = new byte[length];
            random.NextBytes(bytes);
            return bytes;
        }

        protected string GenString(int length = 20)
        {
            var randomChars = Enumerable.Range(1, length)
                .Select(x => letters[random.Next(0, letters.Length)])
                .ToArray();
            return new string(randomChars);
        }
    }
}