using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Polling;
using Telegram.Bot;
using TelegrammBot.Configuration;
using Telegramm.Implementations;
using Microsoft.Extensions.Logging;
using MediatR;
using AutoMapper;
using TelegrammBot.Services.Mapping;
using TelegrammBot.Infrastructure;
using TelegrammBot.Domain.EntitiesDto;
using TelegrammBot.Services.Abstractions.Repositories.Interfaces;
using TelegrammBot.Services.UserService.CommandHandlers;
using TelegrammBot.Services.UserService.Commands;
using TelegrammBot.Infrastructure.Implementation.Repositories;

namespace TelegrammBot
{
    internal static class Registrar
    {
        internal static IServiceCollection AddServices(this IServiceCollection services, HostBuilderContext context, IConfiguration configuration)
        {
            return services
                .AddSingleton((IConfigurationRoot)configuration)
                .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly))
                .AddSingleton<IMapper>(new Mapper(GetMapperConfiguration()))
                .AddInfrastructureServices(configuration)
                .InstallHandlers()
                .InstallRepositories()
                .AddBotServices(context);            
        }

        private static MapperConfiguration GetMapperConfiguration()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<UserProfile>();
            });
            configuration.AssertConfigurationIsValid();
            return configuration;
        }

        private static IServiceCollection InstallHandlers(this IServiceCollection serviceCollection)
        {
            serviceCollection
                //user
                .AddTransient<IRequestHandler<AddUserAsyncCommand, UserDto>, AddUserHandler>();
            return serviceCollection;
        }

        private static IServiceCollection InstallRepositories(this IServiceCollection serviceCollection)
        {
            serviceCollection
                .AddTransient<IUserRepository, UserRepository>();
            ;
            return serviceCollection;
        }

        private static IServiceCollection AddBotServices(this IServiceCollection services, HostBuilderContext context)
        {
            var conf = context.Configuration;

            services.Configure<BotConfiguration>(
            conf.GetSection(BotConfiguration.Configuration));

            var botToken = conf.GetSection("BotConfiguration:BotToken").Value;
            var myChatID = conf.GetSection("BotConfiguration:MyChatID").Value;

            if (string.IsNullOrEmpty(botToken))
            {
                throw new ArgumentNullException("Bot token is not configured.");
            }

            if (string.IsNullOrEmpty(myChatID))
            {
                throw new ArgumentNullException("Chat ID is not configured.");
            }

            services.AddHttpClient("telegram_bot_client")
                    .AddTypedClient<ITelegramBotClient>((httpClient, sp) =>
                    {
                        BotConfiguration? botConfig = sp.GetConfiguration<BotConfiguration>();
                        TelegramBotClientOptions options = new(botConfig.BotToken);
                        return new TelegramBotClient(options, httpClient);
                    });


            services.AddScoped<UpdateHandler>(sp =>
            {
                var botConfig = sp.GetConfiguration<BotConfiguration>();
                return new UpdateHandler(
                    sp.GetRequiredService<ITelegramBotClient>(),
                    sp.GetRequiredService<ISender>(),
                    sp.GetRequiredService<ILogger<UpdateHandler>>(),
                    botConfig.MyChatID
                    
                );
            });
            services.AddScoped<ReceiverService>();
            services.AddHostedService<PollingService>();

            return services;
        }
    }
}
