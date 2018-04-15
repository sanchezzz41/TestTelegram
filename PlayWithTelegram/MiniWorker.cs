using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace PlayWithTelegram
{
    public class MiniWorker
    {
        private readonly TelegramBotClient _botClient;
        private readonly Resolver _resolver;
        private int _offset { get; set; }
        private readonly Queue<Update> _queue;

        public MiniWorker(TelegramBotClient botClient, Resolver resolver)
        {
            _botClient = botClient;
            _resolver = resolver;
            _queue = new Queue<Update>();
            _offset = 0;
        }

        public async Task GetMessages()
        {
            var updates = await _botClient.GetUpdatesAsync(_offset);
            foreach (var update in updates)
            {
                _queue.Enqueue(update);
                _offset = update.Id + 1;

            }

            if (_queue.Any())

                for (int i = 0; i < _queue.Count; i++)
                {
                    var update = _queue.Dequeue();
                    if (update.Type == UpdateType.MessageUpdate)
                    {
                        if (update.Message.Type == MessageType.TextMessage)
                        {
                            new Task(async () =>
                            {
                                var command = _resolver.Resolve(update.Message);
                                if (command != null)
                                    await command.Handler(update.Message);
                            }).Start();
                        }
                    }
                }
        }
    }
}