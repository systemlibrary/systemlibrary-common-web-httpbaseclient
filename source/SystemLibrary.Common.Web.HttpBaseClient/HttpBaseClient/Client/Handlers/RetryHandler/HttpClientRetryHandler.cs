using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SystemLibrary.Common.Web
{
    partial class HttpBaseClient
    {
        partial class Client
        {
            class HttpClientRetryHandler : DelegatingHandler
            {
                public HttpClientRetryHandler(bool ignoreSslErrors)
                {
                    InnerHandler = new WebRequestHandler();
                    if (ignoreSslErrors && InnerHandler is WebRequestHandler handler)
                    {
                        handler.ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) =>
                        {
                            return true;
                        };
                    }
                }

                protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
                {
                    try
                    {
                        return await base
                            .SendAsync(request, cancellationToken)
                            .ConfigureAwait(false);
                    }
                    catch (TaskCanceledException)
                    {
                        if (request.Method == HttpMethod.Get ||
                            request.Method == HttpMethod.Head ||
                            request.Method == HttpMethod.Trace)
                            throw new RetryRequestException();

                        throw;
                    }
                }
            }
        }
    }
}