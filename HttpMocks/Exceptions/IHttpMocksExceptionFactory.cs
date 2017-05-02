using System;

namespace HttpMocks.Exceptions
{
    public interface IHttpMocksExceptionFactory
    {
        Exception CreateWithDiagnostic(Uri mockUrl, string message, Exception innerException);
    }
}