using System;
using HttpMocks.Implementation.Core;

namespace HttpMocks.Implementation
{
    internal class StartedHttpMockFactory : IStartedHttpMockFactory
    {
        public IStartedHttpMock Create(IHttpListenerWrapper httpListenerWrapper, IHandlingMockQueue handlingMockQueue)
        {
            return new StartedHttpMock(httpListenerWrapper, handlingMockQueue);
        }
    }

    public interface IMockUrlEnumerator
    {
        bool MoveNext();

        Uri Current { get; }
    }

    public interface IMockUrlEnumeratorFactory
    {
        IMockUrlEnumerator CreateSingle(string host, int port);
        IMockUrlEnumerator CreateRandomPorts(string host, int maxTryCount = 10);
        IMockUrlEnumerator CreateSingle(Uri url);
    }

    public class MockUrlEnumeratorFactory : IMockUrlEnumeratorFactory
    {
        private readonly IHttpMockPortGenerator httpMockPortGenerator;

        public MockUrlEnumeratorFactory(IHttpMockPortGenerator httpMockPortGenerator)
        {
            this.httpMockPortGenerator = httpMockPortGenerator;
        }

        public IMockUrlEnumerator CreateSingle(string host, int port)
        {
            return new SingleMockUrlEnumerator(host, port);
        }

        public IMockUrlEnumerator CreateSingle(Uri url)
        {
            return new SingleMockUrlEnumerator(url);
        }

        public IMockUrlEnumerator CreateRandomPorts(string host, int maxTryCount)
        {
            return new RandomPortsMockUrlEnumerator(httpMockPortGenerator, maxTryCount, host);
        }
    }

    public class RandomPortsMockUrlEnumerator : IMockUrlEnumerator
    {
        private readonly IHttpMockPortGenerator httpMockPortGenerator;
        private readonly int maxTryCount;
        private readonly string host;
        private int tryCount;

        public RandomPortsMockUrlEnumerator(IHttpMockPortGenerator httpMockPortGenerator, int maxTryCount, string host)
        {
            this.httpMockPortGenerator = httpMockPortGenerator;
            this.maxTryCount = maxTryCount;
            this.host = host;
        }

        public bool MoveNext()
        {
            if (maxTryCount > tryCount)
            {
                var port = httpMockPortGenerator.GeneratePort();
                Current = new Uri($"http://{host}:{port}/");
                tryCount++;
                return true;
            }

            Current = null;
            return false;
        }

        public Uri Current { get; private set; }
    }

    public class SingleMockUrlEnumerator : IMockUrlEnumerator
    {
        private readonly Uri singleMockUrl;
        private bool isComplete;

        public SingleMockUrlEnumerator(Uri mockUrl)
        {
            singleMockUrl = mockUrl;
            isComplete = false;
        }

        public SingleMockUrlEnumerator(string host, int port)
            : this(new Uri($"http://{host}:{port}/"))
        {
        }

        public bool MoveNext()
        {
            if (!isComplete)
            {
                Current = singleMockUrl;
                isComplete = true;
                return true;
            }

            Current = null;
            return false;
        }

        public Uri Current { get; private set; }
    }
}