using System;

namespace HttpMocks.Implementation.Core
{
    public interface IHttpListenerWrapperFactory
    {
        IHttpListenerWrapper Create(Uri prefix);
    }
}