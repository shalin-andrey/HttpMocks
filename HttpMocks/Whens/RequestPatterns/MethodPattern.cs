using HttpMocks.Whens.RequestPatterns.MethodPatterns;

namespace HttpMocks.Whens.RequestPatterns
{
    public static class MethodPattern
    {
        public static AnyMethodPattern Any()
        {
            return new AnyMethodPattern();
        }

        public static StandartMethodPattern Standart(string method)
        {
            return new StandartMethodPattern(method);
        }

        public static StandartMethodPattern Standart(HttpRequestMockMethod method)
        {
            return new StandartMethodPattern(method.ToString());
        }
    }
}