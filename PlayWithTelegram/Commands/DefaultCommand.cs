using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace PlayWithTelegram
{
    public class DefaultCommand : ICommand
    {
        public string CommandText { get; }
        private readonly TelegramBotClient _botClient;

        public DefaultCommand(TelegramBotClient botClient)
        {
            _botClient = botClient;
            CommandText = "defaultCommand";
        }

        public async Task Handler(Message message)
        {
            await _botClient.SendTextMessageAsync(message.Chat.Id,
                $"Простите уважаемый {message.From.FirstName}, я не в состояние это \"обработать\".");
        }
    }
}