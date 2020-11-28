using System;
using Microsoft.Extensions.Configuration;

namespace simple_crud.ApplicationConfiguration
{
    public class ApplicationConfigurationProvider
    {
        private static ApplicationConfiguration _configuration;
        private static ApplicationConfigurationProvider _instance;
        public IApplicationConfiguration Configuration => _configuration ?? throw new InvalidOperationException("Configuration was not initialized!");

        public void InitializeConfiguration(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("main");
            var consoleLoggingEnabled = configuration.GetValue("ConsoleLoggingEnabled", "true");

            _configuration = new ApplicationConfiguration(connectionString, consoleLoggingEnabled);
        }

        public static ApplicationConfigurationProvider Instance => _instance ??= new ApplicationConfigurationProvider();

        private class ApplicationConfiguration : IApplicationConfiguration
        {
            internal ApplicationConfiguration(string connectionString, string consoleLoggingEnabled)
            {
                ConnectionString = connectionString;
                ConsoleLoggingEnabled =  bool.Parse(consoleLoggingEnabled);
            }

            public string ConnectionString { get; }
            public bool ConsoleLoggingEnabled { get; }
        }
    }

    public interface IApplicationConfiguration
    {
        string ConnectionString { get; }
        bool ConsoleLoggingEnabled { get; }
    }
}
