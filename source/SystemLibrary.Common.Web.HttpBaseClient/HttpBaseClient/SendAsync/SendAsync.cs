using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace SystemLibrary.Common.Web
{
    partial class HttpBaseClient
    {
        async Task<ClientResponse<T>> SendAsync<T>(HttpMethod method, string url, object data, MediaType mediaType, int timeoutMilliseconds, IDictionary<string, string> headers, JsonSerializerOptions jsonSerializerOptions, CancellationToken cancellationToken)
        {
            if (timeoutMilliseconds == 20000 && TimeoutMilliseconds != 20000)
                timeoutMilliseconds = TimeoutMilliseconds;

            if (url.IsNot())
                throw new System.Exception("Url is missing when trying to make a " + method + " request");

            var content = Content.GetContent(data, mediaType, jsonSerializerOptions);

            var requestOptions = new RequestOptions()
            {
                Method = method,
                Url = url,
                MediaType = mediaType,
                Headers = headers,
                Content = content,
                RetryOnceOnRequestCancelled = RetryOnceOnRequestCancelled,
                IgnoreSslErrors = IgnoreSslErrors,
                ForceNewClient = false,
                TimeoutMilliseconds = timeoutMilliseconds,
                CancellationToken = cancellationToken
            };

            var response = await Request.SendRequestAsync(requestOptions).ConfigureAwait(false);

            var responseData = await ReadResponseAsync<T>(response, cancellationToken, jsonSerializerOptions).ConfigureAwait(false);

            return new ClientResponse<T>(response, responseData);
        }
    }
}
