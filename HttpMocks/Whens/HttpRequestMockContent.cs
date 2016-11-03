namespace HttpMocks
{
    public class HttpRequestMockContent
    {
        public static readonly HttpRequestMockContent Empty = new HttpRequestMockContent(new byte[0], string.Empty);

        public HttpRequestMockContent(byte[] bytes, string type)
        {
            Bytes = bytes;
            Type = type;
        }

        public byte[] Bytes { get; private set; }
        public string Type { get; private set; }
    }
}