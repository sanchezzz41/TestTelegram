using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace PlayWithTelegram.Commands
{
    public class ListUsersInTheGameCommand : ICommand
    {
        public string CommandText { get; }
        private readonly TelegramBotClient _botClient;
        private readonly ListUser _users;

        public ListUsersInTheGameCommand(TelegramBotClient botClient, ListUser users)
        {
            _botClient = botClient;
            _users = users;
            CommandText = "/listUsers";
        }

        public async Task Handler(Message message)
        {
            var users = _users.GetUsersByTheChatId(message.Chat.Id);
            var resultText = "Список участников:\n";
            if (!users.Any())
            {
                await _botClient.SendTextMessageAsync(message.Chat.Id, "Сорян, никого нет в игре");
                return;
            }
            foreach (var item in users)
            {
                var user = await _botClient.GetChatMemberAsync(item.ChatId, item.UserId);
                resultText += $"{user.User.FirstName}\n";
            }
            await _botClient.SendTextMessageAsync(message.Chat.Id, resultText);
        }
    }
}