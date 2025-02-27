using System;
using HandlebarsDotNet;
using JetBrains.Annotations;
using Newtonsoft.Json;
using WireMock.Handlers;
using WireMock.Logging;
#if USE_ASPNETCORE
using Microsoft.Extensions.DependencyInjection;
#endif

namespace WireMock.Settings
{
    /// <summary>
    /// WireMockServerSettings
    /// </summary>
    public class WireMockServerSettings : IWireMockServerSettings
    {
        /// <inheritdoc cref="IWireMockServerSettings.Port"/>
        [PublicAPI]
        public int? Port { get; set; }

        /// <inheritdoc cref="IWireMockServerSettings.UseSSL"/>
        [PublicAPI]
        public bool? UseSSL { get; set; }

        /// <inheritdoc cref="IWireMockServerSettings.StartAdminInterface"/>
        [PublicAPI]
        public bool? StartAdminInterface { get; set; }

        /// <inheritdoc cref="IWireMockServerSettings.ReadStaticMappings"/>
        [PublicAPI]
        public bool? ReadStaticMappings { get; set; }

        /// <inheritdoc cref="IWireMockServerSettings.WatchStaticMappings"/>
        [PublicAPI]
        public bool? WatchStaticMappings { get; set; }

        /// <inheritdoc cref="IWireMockServerSettings.WatchStaticMappingsInSubdirectories"/>
        [PublicAPI]
        public bool? WatchStaticMappingsInSubdirectories { get; set; }

        /// <inheritdoc cref="IWireMockServerSettings.ProxyAndRecordSettings"/>
        [PublicAPI]
        public IProxyAndRecordSettings ProxyAndRecordSettings { get; set; }

        /// <inheritdoc cref="IWireMockServerSettings.Urls"/>
        [PublicAPI]
        public string[] Urls { get; set; }

        /// <inheritdoc cref="IWireMockServerSettings.StartTimeout"/>
        [PublicAPI]
        public int StartTimeout { get; set; } = 10000;

        /// <inheritdoc cref="IWireMockServerSettings.AllowPartialMapping"/>
        [PublicAPI]
        public bool? AllowPartialMapping { get; set; }

        /// <inheritdoc cref="IWireMockServerSettings.AdminUsername"/>
        [PublicAPI]
        public string AdminUsername { get; set; }

        /// <inheritdoc cref="IWireMockServerSettings.AdminPassword"/>
        [PublicAPI]
        public string AdminPassword { get; set; }

        /// <inheritdoc cref="IWireMockServerSettings.AdminAzureADTenant"/>
        [PublicAPI]
        public string AdminAzureADTenant { get; set; }

        /// <inheritdoc cref="IWireMockServerSettings.AdminAzureADAudience"/>
        [PublicAPI]
        public string AdminAzureADAudience { get; set; }

        /// <inheritdoc cref="IWireMockServerSettings.RequestLogExpirationDuration"/>
        [PublicAPI]
        public int? RequestLogExpirationDuration { get; set; }

        /// <inheritdoc cref="IWireMockServerSettings.MaxRequestLogCount"/>
        [PublicAPI]
        public int? MaxRequestLogCount { get; set; }

        /// <inheritdoc cref="IWireMockServerSettings.PreWireMockMiddlewareInit"/>
        [PublicAPI]
        [JsonIgnore]
        public Action<object> PreWireMockMiddlewareInit { get; set; }

        /// <inheritdoc cref="IWireMockServerSettings.PostWireMockMiddlewareInit"/>
        [PublicAPI]
        [JsonIgnore]
        public Action<object> PostWireMockMiddlewareInit { get; set; }

#if USE_ASPNETCORE
        /// <inheritdoc cref="IWireMockServerSettings.AdditionalServiceRegistration"/>
        [PublicAPI]
        [JsonIgnore]
        public Action<IServiceCollection> AdditionalServiceRegistration { get; set; }
#endif

        /// <inheritdoc cref="IWireMockServerSettings.Logger"/>
        [PublicAPI]
        [JsonIgnore]
        public IWireMockLogger Logger { get; set; }

        /// <inheritdoc cref="IWireMockServerSettings.FileSystemHandler"/>
        [PublicAPI]
        [JsonIgnore]
        public IFileSystemHandler FileSystemHandler { get; set; }

        /// <inheritdoc cref="IWireMockServerSettings.HandlebarsRegistrationCallback"/>
        [PublicAPI]
        [JsonIgnore]
        public Action<IHandlebars, IFileSystemHandler> HandlebarsRegistrationCallback { get; set; }

        /// <inheritdoc cref="IWireMockServerSettings.AllowCSharpCodeMatcher"/>
        [PublicAPI]
        public bool? AllowCSharpCodeMatcher { get; set; }

        /// <inheritdoc cref="IWireMockServerSettings.AllowBodyForAllHttpMethods"/>
        [PublicAPI]
        public bool? AllowBodyForAllHttpMethods { get; set; }

        /// <inheritdoc cref="IWireMockServerSettings.AllowOnlyDefinedHttpStatusCodeInResponse"/>
        [PublicAPI]
        public bool? AllowOnlyDefinedHttpStatusCodeInResponse { get; set; }

        /// <inheritdoc cref="IWireMockServerSettings.DisableJsonBodyParsing"/>
        [PublicAPI]
        public bool? DisableJsonBodyParsing { get; set; }

        /// <inheritdoc cref="IWireMockServerSettings.DisableRequestBodyDecompressing"/>
        [PublicAPI]
        public bool? DisableRequestBodyDecompressing { get; set; }

        /// <inheritdoc cref="IWireMockServerSettings.HandleRequestsSynchronously"/>
        [PublicAPI]
        public bool? HandleRequestsSynchronously { get; set; }

        /// <inheritdoc cref="IWireMockServerSettings.ThrowExceptionWhenMatcherFails"/>
        [PublicAPI]
        public bool? ThrowExceptionWhenMatcherFails { get; set; }

        /// <inheritdoc cref="IWireMockServerSettings.CertificateSettings"/>
        [PublicAPI]
        public IWireMockCertificateSettings CertificateSettings { get; set; }

        /// <inheritdoc cref="IWireMockServerSettings.CustomCertificateDefined"/>
        [PublicAPI]
        public bool CustomCertificateDefined => CertificateSettings?.IsDefined == true;

        /// <inheritdoc cref="IWireMockServerSettings.WebhookSettings"/>
        [PublicAPI]
        public IWebhookSettings WebhookSettings { get; set; }

        /// <inheritdoc cref="IWireMockServerSettings.UseRegexExtended"/>
        [PublicAPI]
        public bool? UseRegexExtended { get; set; } = true;

        /// <inheritdoc cref="IWireMockServerSettings.SaveUnmatchedRequests"/>
        [PublicAPI]
        public bool? SaveUnmatchedRequests { get; set; }
    }
}