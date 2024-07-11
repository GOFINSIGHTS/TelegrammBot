using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Polling;
using Telegram.Bot;
using Telegramm.Abstractions;

namespace Telegramm.Implementations
{
    public class ReceiverService(
        ITelegramBotClient botClient,
        UpdateHandler updateHandler,
        ILogger<ReceiverServiceBase<UpdateHandler>> logger) : ReceiverServiceBase<UpdateHandler>(botClient, updateHandler, logger)
    {
    }
}
