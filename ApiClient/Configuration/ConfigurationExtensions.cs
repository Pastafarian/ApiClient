using System;
using System.Linq;
using Avalara.ApiClient.Authorization.OAuth;
using Microsoft.Extensions.Configuration;

namespace Avalara.ApiClient.Configuration
{
    public static class ConfigurationExtensions
    {
        /// <summary>
        /// The default section where settings are read from the IConfiguration object. This is set to "IdServerSettings".
        /// </summary>
        public const string DefaultConfigSection = "IdServerSettings";

        /// <summary>
        /// Constructs an IdServerSettings instance with the options specified in the "IdServerSettings" section in the IConfiguration object.
        /// </summary>
        public static IdServerSettings GetIdServerSettings(this IConfiguration config)
        {
            return GetIdServerSettings(config, DefaultConfigSection);
        }

        /// <summary>
        /// Constructs an IdServerSettings instance with the options specified in section named by configSection in the IConfiguration object.
        /// </summary>
        public static IdServerSettings GetIdServerSettings(this IConfiguration config, string configSection)
        {
            var idServerSettings = new IdServerSettings();

            IConfiguration section = string.IsNullOrEmpty(configSection) ? config : config.GetSection(configSection);

            if (section == null || !section.GetChildren().Any())
                return null;

            idServerSettings.IdServerUrl = section["IdServerUrl"];
            idServerSettings.IdServerClientId = section["IdServerClientId"];
            idServerSettings.IdServerClientSecret = section["IdServerClientSecret"];
            idServerSettings.Scope = section["Scope"];
            idServerSettings.RequireHttps = section["RequireHttps"] == null ? false : Convert.ToBoolean(section["RequireHttps"]);
            
            return idServerSettings;
        }
    }
}
