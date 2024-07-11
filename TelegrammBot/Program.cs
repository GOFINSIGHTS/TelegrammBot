using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TelegrammBot;
using TelegrammBot.Infrastructure;

IHost host = Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, config) =>
            {
                var env = context.HostingEnvironment;
                var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                      .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);

                config.AddEnvironmentVariables();
                if (environment is not null && environment.Equals("development", StringComparison.OrdinalIgnoreCase))
                {
                    config.AddUserSecrets<Program>();
                }


            })
            .ConfigureServices((context, services) =>
            {
                services.AddServices(context, context.Configuration);                
            })
            .Build();

host.Services.InitializeInfrastructureServices();

host.Run();
