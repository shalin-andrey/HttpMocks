namespace HttpMocks.Implementation.Core
{
    public interface IHttpListenerWrapperFactory
    {
        IHttpListenerWrapper CreateAndStart(IMockUrlEnumerator mockUrlEnumerator);
    }
}