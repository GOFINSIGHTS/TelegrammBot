using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using TelegrammBot.Infrastructure.PostgreSql.Persistance;
using TelegrammBot.Services.Abstractions.Interfaces;

namespace TelegrammBot.Infrastructure
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            var portString = configuration["PostgresPort"];
            portString = string.IsNullOrEmpty(portString) ? "6432" : portString;
            int port = int.Parse(portString);

            var conStrBuilder = new NpgsqlConnectionStringBuilder(configuration.GetConnectionString("TelegrammContext"))
            {
                Password = configuration["PostgresPassword"],
                Host = configuration["PostgresHost"],
                Port = port,
                Username = configuration["PostgresUsername"],
                Database = configuration["PostgresDatabase"]
            };
            var workTitleContext = conStrBuilder.ConnectionString;
            services.AddDbContext<TelegrammContext>(options => options.UseNpgsql(workTitleContext
                , x => x.MigrationsAssembly("TelegrammBot.Infrastructure.PostgreSql")));

            services.AddScoped<IApplicationContext>(provider => provider.GetRequiredService<TelegrammContext>());

            return services;
        }

        public static async void InitializeInfrastructureServices(this IServiceProvider provider)
        {
            using var scope = provider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<TelegrammContext>();
            await dbContext.Database.MigrateAsync();
        }
    }
}
