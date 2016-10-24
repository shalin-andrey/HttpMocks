namespace HttpMocks.Thens
{
    internal class HttpResponseMockContent
    {
        public static readonly HttpResponseMockContent Empty = new HttpResponseMockContent(new byte[0], string.Empty);

        public HttpResponseMockContent(byte[] bytes, string type)
        {
            Bytes = bytes;
            Type = type;
        }

        public byte[] Bytes { get; private set; }
        public string Type { get; private set; }
    }
}