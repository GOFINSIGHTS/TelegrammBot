using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TelegrammBot.Domain.EntitiesDto;
using TelegrammBot.Services.UserService.Commands;

namespace Telegramm.Implementations
{
    public class UpdateHandler : IUpdateHandler
    {
        private readonly ITelegramBotClient _botClient;
        private readonly ISender _sender;
        private readonly ILogger<UpdateHandler> _logger;
        private readonly string _myChat;

        public UpdateHandler(ITelegramBotClient botClient, ISender sender, ILogger<UpdateHandler> logger, string myChat)
        {
            _botClient = botClient ?? throw new ArgumentNullException(nameof(sender), "Uninitialized property");
            _logger = logger ?? throw new ArgumentNullException(nameof(sender), "Uninitialized property");
            _myChat = myChat ?? throw new ArgumentNullException(nameof(sender), "Uninitialized property");
            _sender = sender ?? throw new ArgumentNullException(nameof(sender), "Uninitialized property");

        }

        public async Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var errorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            _logger.LogError(errorMessage);
            await Task.CompletedTask;
        }

        public Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Type != UpdateType.Message || update.Message!.Type != MessageType.Text)
                return Task.CompletedTask;

            Task.Run(() => ProcessUpdate(update, cancellationToken), cancellationToken);

            return Task.CompletedTask;
        }

        private async Task ProcessUpdate(Update update, CancellationToken cancellationToken)
        {
            var message = update.Message;
            _logger.LogInformation($"Received a '{message?.Text}' message in chat {message?.Chat.Id}.");

            try
            {
                if (message.Text!.StartsWith("/start"))
                {
                    await HandleStartCommand(message, cancellationToken);
                }
                else
                {
                    await HandleFeedback(message, cancellationToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception while handling update: {ex}");
            }
        }

        private async Task HandleStartCommand(Message message, CancellationToken cancellationToken)
        {
            const string welcomeMessage = "Добро пожаловать в официальный бот сайта GOFINSIGHTS.COM! 🎉\r\n\r\nМы рады, что вы с нами. Здесь вы можете оставить свои отзывы и предложения по улучшению нашего ресурса. Ваше мнение очень важно для нас, и мы стремимся сделать наш сайт лучшим местом для изучения паттернов проектирования.\r\n\r\nЧтобы оставить отзыв или предложение, просто отправьте нам сообщение. Мы внимательно рассмотрим каждое ваше обращение и постараемся сделать наш сайт еще лучше!\r\n\r\nЕсли у вас есть вопросы или вы хотите узнать больше о паттернах проектирования, посетите наш сайт: GOFINSIGHTS.COM.\r\n\r\nСпасибо, что помогаете нам развиваться! 🙌";
            await _botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: welcomeMessage,
                cancellationToken: cancellationToken);
        }

        private async Task HandleFeedback(Message message, CancellationToken cancellationToken)
        {
            const string feedbackReceivedMessage = "Спасибо за ваш отзыв! 🙏\r\n\r\nМы ценим ваше мнение и обязательно учтем ваши предложения. Ваш вклад помогает нам улучшать наш ресурс и делать его более полезным для всех пользователей.\r\n\r\nЕсли у вас есть еще идеи или замечания, не стесняйтесь делиться ими с нами. Вместе мы сможем создать лучшее место для изучения паттернов проектирования.\r\n\r\nСпасибо, что помогаете нам становиться лучше! 🌟";
            
            await _botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: feedbackReceivedMessage,
                cancellationToken: cancellationToken);

            await _botClient.SendTextMessageAsync(
                chatId: _myChat,
                text: message.Text,
                cancellationToken: cancellationToken);

            await _sender.Send(new AddUserAsyncCommand(new UserDto() { ChatId = message.Chat.Id, Message = message.Text, Date = DateTime.UtcNow}),cancellationToken);
        }
    }

}
