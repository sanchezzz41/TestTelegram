using System;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace PlayWithTelegram
{
    public class TelegramWaitMessage
    {
        public bool IsWaitGameName { get; set; }
        public Action<TelegramBotClient, Message> Action { get; set; }

        public string Test { get; set; }

        public TelegramWaitMessage()
        {
            Action = async (client, message) =>
            {
                Test = message.Text.Substring(1);
                await client.SendTextMessageAsync(message.Chat.Id, $"Прозвище {Test} успешно установлено");
                IsWaitGameName = false;
            };
        }
    }
}