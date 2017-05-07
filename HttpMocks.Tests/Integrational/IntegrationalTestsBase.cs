using System;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using NUnit.Framework;

namespace HttpMocks.Tests.Integrational
{
    public class IntegrationalTestsBase
    {
        protected HttpMockRepository HttpMocks;

        [SetUp]
        public void SetUp()
        {
            HttpMocks = new HttpMockRepository();
        }

        [TearDown]
        public virtual void TearDown()
        {
            HttpMocks.VerifyAll();
        }

        protected readonly Uri DefaultMockUrl = new Uri("http://localhost:3465/");

        protected static Uri BuildUrl(Uri mockUrl, string path, NameValueCollection query = null)
        {
            var uriBuilder = new UriBuilder(mockUrl.Scheme, mockUrl.Host, mockUrl.Port, path);

            if (query != null)
            {
                uriBuilder.Query = string.Join("&", query.AllKeys.Select(x => $"{x}={query[(string) x]}"));
            }

            return uriBuilder.Uri;
        }

        protected static TestResponse Send(Uri url, string method, byte[] contentBytes = null, string contentType = null, NameValueCollection headers = null)
        {
            try
            {
                var request = WebRequest.Create(url);
                request.Method = method;

                if (headers != null)
                {
                    foreach (var headerName in headers.AllKeys)
                    {
                        request.Headers.Add(headerName, headers[headerName]);
                    }
                }

                if (contentBytes != null && contentBytes.Length > 0)
                {
                    using (var stream = request.GetRequestStream())
                    {
                        stream.Write(contentBytes, 0, contentBytes.Length);
                    }
                    if (contentType != null)
                    {
                        request.ContentType = contentType;
                    }
                }

                request.Timeout = 2000;

                var httpWebResponse = (HttpWebResponse)request.GetResponse();
                return Convert(httpWebResponse);
            }
            catch (WebException e)
            {
                if (e.Response != null)
                {
                    return Convert((HttpWebResponse)e.Response);
                }

                return TestResponse.Create(452);
            }
        }

        private static TestResponse Convert(HttpWebResponse httpWebResponse)
        {
            return TestResponse.Create((int)httpWebResponse.StatusCode, ReadResponseContentBytes(httpWebResponse));
        }

        private static byte[] ReadResponseContentBytes(HttpWebResponse httpWebResponse)
        {
            if (httpWebResponse.ContentLength <= 0)
            {
                return new byte[0];
            }

            var mem = new MemoryStream();
            using (var stream = httpWebResponse.GetResponseStream())
            {
                stream?.CopyTo(mem, (int)httpWebResponse.ContentLength);
                return mem.ToArray();
            }
        }
    }
}