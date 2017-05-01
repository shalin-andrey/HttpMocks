Http Mocks
================

Http Mocks is a framework for integration tests.

Create HttpMock
----
Create http mock, run for host and port (start http server) and verify it (stop http server).
```csharp
var httpMocks = new HttpMockRepository();
using (var httpMock = httpMocks.New(host, port))
{
    /* mock */
}
httpMocks.VerifyAll()
```

Mock POST method with content
----
Create mock of http request:
```
POST /users/ HTTP/1.1
Host: github.com

<...>
```
```csharp
httpMock
    .WhenRequestPost("/users")
    .Content(postContentBytes, "application/text")
    .ThenResponse(200);
```