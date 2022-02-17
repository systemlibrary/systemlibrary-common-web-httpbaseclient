using SystemLibrary.Common.Net;

namespace SystemLibrary.Common.Web
{
    /// <summary>
    /// Override default configurations in 'SystemLibrary.Common.Web.HttpBaseClient' by adding 'systemLibraryCommonWebHttpBaseClient' object to 'appSettings.json'
    /// </summary>
    /// <example>
    /// 'appSettings.json'
    /// <code class="language-csharp hljs">
    /// {
    ///     ...,
    ///     "systemLibraryCommonWebHttpBaseClient": {
    ///         "retryRequestTimeoutSeconds": 10,
    ///         "cacheClientConnectionSeconds": 300
    ///     },
    ///     ...
    /// }
    /// </code>
    /// </example>
    public class AppSettingsConfig : Config<AppSettingsConfig>
    {
        public class Configuration
        {
            public int RetryRequestTimeoutSeconds { get; set; } = 10;
            public int CacheClientConnectionSeconds { get; set; } = 300;
        }

        public AppSettingsConfig()
        {
            SystemLibraryCommonWebHttpBaseClient = new Configuration();
        }

        public Configuration SystemLibraryCommonWebHttpBaseClient { get; set; }
    }
}
