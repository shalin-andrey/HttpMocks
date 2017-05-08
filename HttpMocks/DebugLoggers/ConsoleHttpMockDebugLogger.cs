using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using HttpMocks.Implementation;

namespace HttpMocks.DebugLoggers
{
    public class ConsoleHttpMockDebugLogger : HttpMockDebugLoggerBase
    {
        public override void LogHttpRequest(HttpRequestInfo request)
        {
            Console.WriteLine($"HM.Request: {request.Method} {request.Path}?{HttpQueryToString(request.Query)}");
            foreach (var headerName in request.Headers.AllKeys)
            {
                var headerValue = request.Headers[headerName];
                Console.WriteLine($"HM.Request: {headerName}: {headerValue}");
            }

            Console.WriteLine("HM.Request:");
            Console.WriteLine($"HM.Request: {request.ContentBytes.Length}");
        }

        public override void LogHttpResponse(HttpResponseInfo response)
        {
            Console.WriteLine($"HM.Response: HTTP/1.1 {response.StatusCode}");
            foreach (var headerName in response.Headers.AllKeys)
            {
                var headerValue = response.Headers[headerName];
                Console.WriteLine($"HM.Response: {headerName}: {headerValue}");
            }

            Console.WriteLine("HM.Response:");
            Console.WriteLine($"HM.Response: {response.ContentBytes.Length}");
        }

        private string HttpQueryToString(NameValueCollection query)
        {
            var parameterAndValuePairs = new List<string>();
            foreach (var parameterName in query.AllKeys)
            {
                parameterAndValuePairs.Add($"{parameterName}={query[parameterName]}");
            }
            return string.Join("&", parameterAndValuePairs);
        }
    }
}