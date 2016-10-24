using System;

namespace HttpMocks.Verifications
{
    [Serializable]
    public class AssertHttpMockException : Exception
    {
        public AssertHttpMockException(string message)
            : base(message)
        {
        }
    }
}