using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace PlayWithTelegram
{
    public class GetRandomStickerCommand : ICommand
    {
        public string CommandText { get; }
        private readonly TelegramBotClient _botClient;


        public GetRandomStickerCommand(TelegramBotClient botClient)
        {
            _botClient = botClient;
            CommandText = "/sticker";
        }

        public async Task Handler(Message message)
        {
            var stickers = await _botClient.GetStickerSetAsync("konosubarashi");
            var random = new Random();

            var strickerId = stickers.Stickers[random.Next(0, stickers.Stickers.Count)].FileId;
            await _botClient.SendStickerAsync(message.Chat.Id, new FileToSend(strickerId));
        }
    }
}