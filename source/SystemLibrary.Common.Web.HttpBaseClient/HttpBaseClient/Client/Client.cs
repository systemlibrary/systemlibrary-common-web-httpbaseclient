﻿using System;
using System.Collections.Concurrent;
using System.Net.Http;

namespace SystemLibrary.Common.Web
{
    partial class HttpBaseClient
    {
        partial class Client
        {
            static int _ClientExpiresInSeconds = -1;
            static int ClientExpiresInSeconds => _ClientExpiresInSeconds > -1 ? _ClientExpiresInSeconds : 
                (_ClientExpiresInSeconds = AppSettingsConfig.Current.SystemLibraryCommonWebHttpBaseClient.CacheClientConnectionSeconds);

            static ConcurrentDictionary<string, CacheModel> Cache;
            static ConcurrentDictionary<string, CacheModel> DisposeQueue;

            static Client()
            {
                Cache = new ConcurrentDictionary<string, CacheModel>();
                DisposeQueue = new ConcurrentDictionary<string, CacheModel>();
            }

            internal static HttpClient GetClient(string url, int timeoutMilliseconds, bool retryOnRequestCancelled = false, bool forceNewClient = false, bool ignoreSslErrors = false)
            {
                var uri = new Uri(url);

                var key = nameof(HttpBaseClient) + nameof(GetClient) + uri.Scheme + uri.Authority + uri.Port + "#" + timeoutMilliseconds + "#" + retryOnRequestCancelled;

                if (forceNewClient)
                {
                    RemoveFromCache(key);
                }
                else if (Cache.TryGetValue(key, out CacheModel cached))
                {
                    if (HasExpired(cached))
                        RemoveFromCache(key);
                    else
                        return cached.HttpClientCached;
                }

                return New(key, timeoutMilliseconds, retryOnRequestCancelled, ignoreSslErrors);
            }

            static HttpClient New(string key, int timeoutMilliseconds, bool retryOnRequestCancelled, bool ignoreSslErrors)
            {
                var retryRequestHandler = retryOnRequestCancelled ? new HttpClientRetryHandler(ignoreSslErrors) : null;

                var timeoutRequestHandler = new HttpClientTimeoutHandler(timeoutMilliseconds, retryRequestHandler, ignoreSslErrors);

                var httpClientCacheModel = new CacheModel()
                {
                    HttpClientCached = new HttpClient(timeoutRequestHandler, disposeHandler: true),
                    Expires = DateTime.Now.AddSeconds(ClientExpiresInSeconds)
                };

                Dump.Write(AppSettingsConfig.Current.SystemLibraryCommonWebHttpBaseClient);

                Dump.Write("SECONDS: " + ClientExpiresInSeconds);
                if (ClientExpiresInSeconds > 0)
                {
                    Cache.TryAdd(key, httpClientCacheModel);
                }
                return httpClientCacheModel.HttpClientCached;
            }

            static bool HasExpired(CacheModel httpClientCached)
            {
                return httpClientCached?.HttpClientCached == null || httpClientCached.Expires <= DateTime.Now;
            }

            static void RemoveFromCache(string key)
            {
                Cache.TryRemove(key, out CacheModel httpClientCached);
               
                Dispose();

                if (httpClientCached != null)
                    DisposeQueue.TryAdd(key + DateTime.Now.ToString("hh:mm:ss.fffff"), httpClientCached);
            }

            static void Dispose()
            {
                CleanDisposeQueue();
            }

            static void CleanDisposeQueue()
            {
                var disposedTime = DateTime.Now.AddSeconds(-ClientExpiresInSeconds);
                var keys = DisposeQueue.Keys;
                foreach (var key in keys)
                {
                    try
                    {
                        if (DisposeQueue.TryGetValue(key, out CacheModel queueCached))
                        {
                            if (queueCached?.HttpClientCached != null && queueCached.Expires < disposedTime)
                            {
                                DisposeQueue.TryRemove(key, out _);
                                queueCached?.Dispose();
                            }
                        }
                    }
                    catch
                    {
                    }
                }
            }
        }
    }
}