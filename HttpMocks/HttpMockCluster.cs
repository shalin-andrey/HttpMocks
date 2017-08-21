using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HttpMocks.Implementation;
using HttpMocks.Verifications;
using HttpMocks.Whens;
using HttpMocks.Whens.RequestPatterns;

namespace HttpMocks
{
    internal class HttpMock : IHttpMock
    {
        private readonly IStartedHttpMock[] startedHttpMocks;
        private readonly List<IHttpRequestMockBuilder> internalHttpRequestMockBuilders;
        private readonly IHandlingMockQueue handlingMockQueue;

        internal HttpMock(IStartedHttpMock[] startedHttpMocks, IHandlingMockQueue handlingMockQueue)
        {
            this.startedHttpMocks = startedHttpMocks;
            this.handlingMockQueue = handlingMockQueue;

            internalHttpRequestMockBuilders = new List<IHttpRequestMockBuilder>();
        }

        public Uri[] MockUris => startedHttpMocks.Select(x => x.MockUrl).ToArray();
        public Uri MockUri => MockUris[0];

        public IHttpRequestMock WhenRequestGet(string path = null)
        {
            return WhenRequestGet(Convert(path));
        }

        public IHttpRequestMock WhenRequestGet(IHttpRequestPathPattern pathPattern)
        {
            return CreateMockBuilder()
                .Method("GET")
                .Path(pathPattern);
        }

        public IHttpRequestMock WhenRequestPost(string path = null)
        {
            return WhenRequestGet(Convert(path));
        }

        public IHttpRequestMock WhenRequestPost(IHttpRequestPathPattern pathPattern)
        {
            return CreateMockBuilder()
                .Method("POST")
                .Path(pathPattern);
        }

        public IHttpRequestMock WhenRequestPut(string path = null)
        {
            return WhenRequestGet(Convert(path));
        }

        public IHttpRequestMock WhenRequestPut(IHttpRequestPathPattern pathPattern)
        {
            return CreateMockBuilder()
                .Method("PUT")
                .Path(pathPattern);
        }

        public IHttpRequestMock WhenRequestDelete(string path = null)
        {
            return WhenRequestGet(Convert(path));
        }

        public IHttpRequestMock WhenRequestDelete(IHttpRequestPathPattern pathPattern)
        {
            return CreateMockBuilder()
                .Method("DELETE")
                .Path(pathPattern);
        }

        public IHttpRequestMock WhenRequestPatch(string path = null)
        {
            return WhenRequestGet(Convert(path));
        }

        public IHttpRequestMock WhenRequestPatch(IHttpRequestPathPattern pathPattern)
        {
            return CreateMockBuilder()
                .Method("PATCH")
                .Path(pathPattern);
        }

        public IHttpRequestMock WhenRequest()
        {
            return CreateMockBuilder();
        }

        public void Run()
        {
            var httpRequestMocks = internalHttpRequestMockBuilders.Select(b => b.Build()).ToArray();
            handlingMockQueue.Enqueue(httpRequestMocks);
        }

        public void Dispose()
        {
            Run();
        }

        public async Task<VerificationResult[]> StopAsync()
        {
            var stopMockTasks = startedHttpMocks
                .Select(startedHttpMock => startedHttpMock.StopAsync())
                .ToArray();

            var verificationResults = await Task.WhenAll(stopMockTasks).ConfigureAwait(false);
            var results = verificationResults.SelectMany(x => x).ToList();

            foreach (var httpRequestMockHandlingInfo in handlingMockQueue.GetNotActual())
            {
                results.Add(VerificationResult.Create($"Request {httpRequestMockHandlingInfo.RequestMock} expected, but not actual. Actual count = {httpRequestMockHandlingInfo.UsageCount}, expect count = {httpRequestMockHandlingInfo.RequestMock.Response.RepeatCount}"));
            }

            return results.ToArray();
        }

        private IHttpRequestPathPattern Convert(string path)
        {
            return path == null
                ? PathPattern.Any() as IHttpRequestPathPattern
                : PathPattern.Smart(path);
        }

        private HttpRequestMockBuilder CreateMockBuilder()
        {
            var requestMockBuilder = new HttpRequestMockBuilder();
            internalHttpRequestMockBuilders.Add(requestMockBuilder);
            return requestMockBuilder;
        }
    }
}