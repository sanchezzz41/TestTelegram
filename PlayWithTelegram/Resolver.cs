using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace PlayWithTelegram
{
    public class Resolver
    {
        private readonly TelegramBotClient _botClient;
        private readonly IServiceProvider _provider;

        public Resolver(TelegramBotClient botClient, IServiceProvider collection)
        {
            _botClient = botClient;
            _provider = collection;
        }

        public ICommand Resolve(Message message)
        {
            if (message.Type == MessageType.TextMessage)
            {
                if (message.Text == null || !message.Text.StartsWith("/"))
                    return null;

                var command = message.Text?.Split(new char[]{'@',' '}).FirstOrDefault()?.ToLower();
                var result = _provider.GetServices<ICommand>()
                    .SingleOrDefault(x => x.CommandText.ToLower().Contains(command));
                if (result == null)
                    return _provider.GetServices<ICommand>()
                        .SingleOrDefault(x => x.CommandText.ToLower() == "defaultcommand");
                return result;
            }

            return _provider.GetServices<ICommand>()
                .Single(x => x.CommandText.ToLower() == "defaultcommand");
        }
    }
}