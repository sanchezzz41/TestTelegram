using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace PlayWithTelegram.Commands
{
    public class MiniGameCommand : ICommand
    {
        public string CommandText { get; }
        private readonly TelegramBotClient _botClient;
        private readonly ListUser _listUser;
     
        public MiniGameCommand(TelegramBotClient botClient, ListUser listUser)
        {
            _botClient = botClient;
            _listUser = listUser;
            CommandText = "/startGame";
        }
     
        public async Task Handler(Message message)
        {
            var random = new Random();
            var users = _listUser.GetUsersByTheChatId(message.Chat.Id);
            if (!users.Any())
            {
                await _botClient.SendTextMessageAsync(message.Chat.Id, "Сорян, никто не зареган для игры :(");
                return;
            }
            var user = users[random.Next(0, users.Count)];
            var member = await _botClient.GetChatMemberAsync(message.Chat.Id, user.UserId);
            
            await _botClient.SendTextMessageAsync(message.Chat.Id, "Так, кто же является крыссой, анализирую...");
            Thread.Sleep(TimeSpan.FromSeconds(2));
            await _botClient.SendTextMessageAsync(message.Chat.Id, $"Крыссой является {member.User.FirstName}.");
        }
    }
}