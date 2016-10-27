namespace HttpMocks.Tests.Integrational
{
    public class TestResponse
    {
        public static TestResponse Create(int statusCode, byte[] contentBytes = null)
        {
            return new TestResponse
            {
                StatusCode = statusCode,
                ContentBytes = contentBytes ?? new byte[0]
            };
        }

        public int StatusCode { get; set; }
        public byte[] ContentBytes { get; set; }
    }
}