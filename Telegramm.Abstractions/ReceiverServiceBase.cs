﻿using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using Telegramm.Abstractions.Interfaces;

namespace Telegramm.Abstractions
{
    public abstract class ReceiverServiceBase<TUpdateHandler>(
        ITelegramBotClient botClient,
        TUpdateHandler updateHandler,
        ILogger<ReceiverServiceBase<TUpdateHandler>> logger) : IReceiverService
    where TUpdateHandler : IUpdateHandler
    {
        private readonly ITelegramBotClient _botClient = botClient;
        private readonly IUpdateHandler _updateHandler = updateHandler;
        private readonly ILogger<ReceiverServiceBase<TUpdateHandler>> _logger = logger;

        /// <summary>
        /// Start to service Updates with provided Update Handler class
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        public async Task ReceiveAsync(CancellationToken stoppingToken)
        {
            // ToDo: we can inject ReceiverOptions through IOptions container
            var receiverOptions = new ReceiverOptions()
            {
                AllowedUpdates = Array.Empty<UpdateType>(),
                ThrowPendingUpdates = true,
            };

            var me = await _botClient.GetMeAsync(stoppingToken);
            _logger.LogInformation("Start receiving updates for {BotName}", me.Username ?? "GOFINSIGHTS Bot");

            // Start receiving updates
            await _botClient.ReceiveAsync(
                updateHandler: _updateHandler,
                receiverOptions: receiverOptions,
                cancellationToken: stoppingToken);
        }
    }
}
