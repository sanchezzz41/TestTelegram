using System;
using System.IO.Pipes;
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
        private readonly TelegramWaitMessage _waitMessage;

        public MiniGameCommand(TelegramBotClient botClient, ListUser listUser, TelegramWaitMessage waitMessage)
        {
            _botClient = botClient;
            _listUser = listUser;
            _waitMessage = waitMessage;
            CommandText = "/startGame";
        }

        public async Task Handler(Message message)
        {
            var onWho = "крыса";
            var arg = message.Text.Split(" ").Where(x => !string.IsNullOrWhiteSpace(x)).Skip(1).FirstOrDefault();
            if (_waitMessage.Test != null)
                onWho = _waitMessage.Test;
            var random = new Random();
            var users = _listUser.GetUsersByTheChatId(message.Chat.Id);
            if (!users.Any())
            {
                await _botClient.SendTextMessageAsync(message.Chat.Id, "Сорян, никто не зареган для игры :(");
                return;
            }

            var user = users[random.Next(0, users.Count)];
            var member = await _botClient.GetChatMemberAsync(message.Chat.Id, user.UserId);

            await _botClient.SendTextMessageAsync(message.Chat.Id,
                $"Так, кто же получит титул \"{onWho}\", анализирую...");
            Thread.Sleep(TimeSpan.FromSeconds(2));
            await _botClient.SendTextMessageAsync(message.Chat.Id,
                $"Титул \"{onWho}\" получает {member.User.FirstName}.");
        }
    }
}