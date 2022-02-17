﻿using System.Threading;
using System.Threading.Tasks;

namespace SystemLibrary.Common.Web.HttpBaseClientTests
{
    class HttpBinClient : HttpBaseClient
    {
        const string firewallClientUrl = "https://172.4.5.6/";
        const string clientUrl = "http://httpbin.org";
        public HttpBinClient(bool retryOnRequestCancelled = false) : base(retryOnRequestCancelled)
        {
        }

        public ClientResponse<string> Head(MediaType mediaType)
        {
            return Head<string>("https://www.google.com", mediaType);
        }

        public ClientResponse<string> Delete(object data, MediaType mediaType)
        {
            return Delete<string>(clientUrl + "/delete", data, mediaType);
        }

        public ClientResponse<string> Put(object data, MediaType mediaType)
        {
            return Put<string>(clientUrl + "/put", data, mediaType);
        }

        public ClientResponse<string> Get()
        {
            return Get<string>(clientUrl + "/get");
        }

        public ClientResponse<string> Post(object data, MediaType mediaType)
        {
            return Post<string>(clientUrl + "/post", data, mediaType);
        }

        public ClientResponse<string> PostUrlEncoded(object data)
        {
            return Post<string>(clientUrl + "/post", data, MediaType.xwwwformUrlEncoded);
        }

        public async Task<ClientResponse<string>> PostAsync(string data)
        {
            return await PostAsync<string>(clientUrl + "/post", data, MediaType.textplain, 10000);
        }

        public ClientResponse<string> Get_Retry_Request_Against_Firewall(CancellationToken cancellationToken = default)
        {
            return Get<string>(firewallClientUrl, MediaType.json, 200, null, null, cancellationToken);
        }

        public ClientResponse<string> Post_Retry_Request_Against_Firewall()
        {
            return Post<string>(firewallClientUrl, "hello world", MediaType.json, 300);
        }

        public ClientResponse<string> GetWithCancellationToken(CancellationToken token)
        {
            return Get<string>(clientUrl + "/delay/2", MediaType.json, 4000, null, null, token);
        }

        public ClientResponse<string> GetWithTimeout(int timeoutMilliseconds)
        {
            return Get<string>(clientUrl + "/delay/1", MediaType.json, timeoutMilliseconds);
        }
    }
}

