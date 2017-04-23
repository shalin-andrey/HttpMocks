namespace HttpMocks.Whens.RequestPatterns.MethodPatterns
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
    }
}