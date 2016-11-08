using System;

namespace HttpMocks.Exceptions
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