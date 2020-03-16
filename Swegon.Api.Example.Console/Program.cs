using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Swegon.Api.Example
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var provider = BuildApp();

            var logger = provider.GetRequiredService<ILogger<Program>>();
            var apiClient = provider.GetRequiredService<SwegonApiClient>();
            var tokenRetriever = provider.GetRequiredService<TokenRetriever>();

            var authResult = await tokenRetriever.AuthResult();

            var salesOrder = await apiClient
                .GetSalesOrderAsync(authResult.AccessToken, "1024757");

            logger.LogInformation("Found Sales Order:");

            logger.LogInformation(JToken.Parse(salesOrder).ToString(Newtonsoft.Json.Formatting.Indented));

            logger.LogInformation("Press any key to exit");
            Console.ReadKey();
            provider.Dispose();
        }

        private static ServiceProvider BuildApp()
        {
            var builder = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.json");

            var configuration = builder.Build();
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                    .AddConsole();
            });

            var serviceProvider = new ServiceCollection();

            serviceProvider
                .AddSingleton(loggerFactory)
                .AddSingleton<IConfiguration>(configuration)
                .AddTransient<TokenRetriever>()
                .AddHttpClient<SwegonApiClient>();

            return serviceProvider.BuildServiceProvider();
        }
    }
}