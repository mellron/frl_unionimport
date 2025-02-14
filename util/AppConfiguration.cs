using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace frl_unionimport.util
{
    public static class AppConfiguration
    {
        private static readonly IConfiguration _configuration;

        // Static constructor builds the configuration once.
        static AppConfiguration()
        {
            string basePath = AppContext.BaseDirectory;
            _configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
        }

        // Retrieves a configuration value by key.
        public static string GetPropertyValue(string key)
        {
            return _configuration[key] ?? string.Empty;
        }

        // Retrieves a connection string by key.
        public static string GetConnectionString(string key)
        {
            return _configuration.GetConnectionString(key) ?? string.Empty;
        }
    }
}
