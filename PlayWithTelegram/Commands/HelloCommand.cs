using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace PlayWithTelegram
{
    public class HelloCommand : ICommand
    {
        public string CommandText { get; }


        private readonly TelegramBotClient _botClient;

        public HelloCommand(TelegramBotClient botClient)
        {
            _botClient = botClient;
            CommandText = "/hello";
        }

        public async Task Handler(Message message)
        {
            await _botClient.SendTextMessageAsync(message.Chat.Id, $"Здравствуйте {message.From?.FirstName}!");
        }
    }
}