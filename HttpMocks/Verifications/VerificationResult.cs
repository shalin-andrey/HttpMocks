namespace HttpMocks.Verifications
{
    public class VerificationResult
    {
        public static VerificationResult Create(string message)
        {
            return new VerificationResult
            {
                Message = message
            };
        }

        public string Message { get; private set; }
    }
}