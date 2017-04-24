﻿using System;
using System.Collections.Generic;
using System.Linq;
using HttpMocks.Whens;

namespace HttpMocks
{
    internal class HttpMock : IHttpMock
    {
        private readonly IHttpMockRunner httpMockRunner;
        private readonly List<IHttpRequestMockBuilder> internalHttpRequestMockBuilders;
        
        internal HttpMock(IHttpMockRunner httpMockRunner, Uri mockUri)
        {
            this.httpMockRunner = httpMockRunner;
            MockUri = mockUri;
            internalHttpRequestMockBuilders = new List<IHttpRequestMockBuilder>();
        }

        public Uri MockUri { get; }

        public IHttpRequestMock WhenRequestGet()
        {
            return WhenRequestGet(string.Empty);
        }

        public IHttpRequestMock WhenRequestGet(string path)
        {
            return CreateMockBuilder()
                .Method("GET")
                .Path(path);
        }

        public IHttpRequestMock WhenRequestPost()
        {
            return WhenRequestPost(string.Empty);
        }

        public IHttpRequestMock WhenRequestPost(string path)
        {
            return CreateMockBuilder()
                .Method("POST")
                .Path(path);
        }

        public IHttpRequestMock WhenRequestPut()
        {
            return WhenRequestPut(string.Empty);
        }

        public IHttpRequestMock WhenRequestPut(string path)
        {
            return CreateMockBuilder()
                .Method("PUT")
                .Path(path);
        }

        public IHttpRequestMock WhenRequestDelete()
        {
            return WhenRequestDelete(string.Empty);
        }

        public IHttpRequestMock WhenRequestDelete(string path)
        {
            return CreateMockBuilder()
                .Method("DELETE")
                .Path(path);
        }

        public IHttpRequestMock WhenRequestPatch()
        {
            return WhenRequestPatch(string.Empty);
        }

        public IHttpRequestMock WhenRequestPatch(string path)
        {
            return CreateMockBuilder()
                .Method("PATCH")
                .Path(path);
        }

        public void Run()
        {
            var httpRequestMocks = internalHttpRequestMockBuilders.Select(b => b.Build()).ToArray();
            httpMockRunner.RunMocks(MockUri, httpRequestMocks);
        }

        public void Dispose()
        {
            Run();
        }

        private HttpRequestMockBuilder CreateMockBuilder()
        {
            var requestMockBuilder = new HttpRequestMockBuilder();
            internalHttpRequestMockBuilders.Add(requestMockBuilder);
            return requestMockBuilder;
        }
    }
}