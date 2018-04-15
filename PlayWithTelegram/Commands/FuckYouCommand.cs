using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace PlayWithTelegram
{
    public class FuckYouCommand : ICommand
    {
        public string CommandText { get; }
        private readonly TelegramBotClient _botClient;


        public FuckYouCommand(TelegramBotClient botClient)
        {
            _botClient = botClient;
            CommandText = "/LoveYou";
        }

        public async Task Handler(Message message)
        {
            var random = new Random();
            var number = random.Next(0, 2);
            var result = number == 1
                ? $"Я тебя тоже люблю, сладенький мой {message.From.FirstName}"
                : $"Вадимка любит только Саню! Так что, {message.From.FirstName} иди нахуй!";
            await _botClient.SendTextMessageAsync(message.Chat.Id, result);
        }
    }
}