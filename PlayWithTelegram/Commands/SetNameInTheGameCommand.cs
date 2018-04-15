using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace PlayWithTelegram
{
    public class SetNameInTheGameCommand : ICommand
    {
        public string CommandText { get; }

        private readonly TelegramWaitMessage _waitMessage;
        private readonly TelegramBotClient _botClient;

        public SetNameInTheGameCommand(TelegramBotClient botClient, TelegramWaitMessage waitMessage)
        {
            _botClient = botClient;
            _waitMessage = waitMessage;
            CommandText = "/setName";
        }

        public async Task Handler(Message message)
        {
            _waitMessage.IsWaitGameName = true;
            await _botClient.SendTextMessageAsync(message.Chat.Id, $"Пожалуйста введите прозвище, на которое хотите скатнуть");
        }
    }
}