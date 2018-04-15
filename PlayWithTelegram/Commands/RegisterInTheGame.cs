using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace PlayWithTelegram.Commands
{
    public class RegisterInTheGameCommand : ICommand
    {
        public string CommandText { get; }
        private readonly TelegramBotClient _botClient;
        private readonly ListUser _users;

        public RegisterInTheGameCommand(TelegramBotClient botClient, ListUser users)
        {
            _botClient = botClient;
            _users = users;
            CommandText = "/register";
        }

        public async Task Handler(Message message)
        {
            if (_users.AddUserId(message.Chat.Id, message.From.Id))
                await _botClient.SendTextMessageAsync(message.Chat.Id,
                    $"Поздравляю {message.From.FirstName}, вы успешно зарегестрировались :3");
            else
                await _botClient.SendTextMessageAsync(message.Chat.Id,
                    $"Сорян {message.From.FirstName}, вы уже зареганы в игре :(");
        }

    }
}