using System;
using System.Threading;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.AzureAppServices;
using simple_crud.ApplicationConfiguration;
using simple_crud.Data.Contexts;
using simple_crud.Data.DatabaseInitializer;

namespace simple_crud
{
    public class Program
    {
        private static readonly TimeSpan DatabaseInitTimeout = TimeSpan.FromSeconds(30);

        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            InitializeDb(host);

            host.Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            var builder = Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); })
                .ConfigureLogging(ConfigureLogging)
                .ConfigureServices(ConfigureServices);

            return builder;
        }

        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            if (ApplicationConfiguration.AzureLoggingEnabled)
                serviceCollection.Configure<AzureFileLoggerOptions>(options =>
                {
                    options.FileName = "azure-diagnostics-";
                    options.FileSizeLimit = 50 * 1024;
                    options.RetainedFileCountLimit = 5;
                });

            serviceCollection.AddSwaggerGen();
            serviceCollection.AddDbContext<NoveltyContext>(options => options.UseSqlServer(ApplicationConfiguration.ConnectionString));
        }

        private static void InitializeDb(IHost host)
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            var logger = services.GetService<ILogger<Program>>();

            try
            {
                var ctx = services.GetService<NoveltyContext>();
                var cancellationToken = new CancellationTokenSource(DatabaseInitTimeout).Token;
                DatabaseInitializer.Initialize(ctx, logger, cancellationToken).Wait(cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Exception raised during database initialization!");
                throw;
            }
        }

        private static void ConfigureLogging(ILoggingBuilder logging)
        {
            logging.ClearProviders();

            if (ApplicationConfiguration.ConsoleLoggingEnabled)
                logging.AddConsole();
            if (ApplicationConfiguration.AzureLoggingEnabled)
                logging.AddAzureWebAppDiagnostics();
        }

        private static IApplicationConfiguration _applicationConfiguration;

        private static IApplicationConfiguration ApplicationConfiguration => _applicationConfiguration ??=
            ApplicationConfigurationProvider.Instance.Configuration;
    }
}
