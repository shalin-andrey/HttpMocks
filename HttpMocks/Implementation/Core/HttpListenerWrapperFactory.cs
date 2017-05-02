using System;

namespace HttpMocks.Implementation.Core
{
    internal class HttpListenerWrapperFactory : IHttpListenerWrapperFactory
    {
        public IHttpListenerWrapper Create(Uri prefix)
        {
            return new HttpListenerWrapper(prefix);
        }
    }
}