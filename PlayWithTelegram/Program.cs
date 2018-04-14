using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace PlayWithTelegram
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();

            var botClient = new TelegramBotClient("553480406:AAETczW5qnJp4ZkbQ2_E77l2oyzP3IF9qig");

            serviceCollection.AddScoped<ICommand, HelloCommand>();
            serviceCollection.AddScoped<ICommand, FuckYouCommand>();
            serviceCollection.AddScoped<ICommand, DefaultCommand>();
            serviceCollection.AddScoped<ICommand, GetRandomStickerCommand>();
            serviceCollection.AddSingleton(botClient);
            serviceCollection.AddSingleton<Resolver>();
            serviceCollection.AddSingleton<MiniWorker>();
            var provider = serviceCollection.BuildServiceProvider();
            Test(provider).Wait();
        }

        public static async Task Test(IServiceProvider prov)
        {
            var worker = prov.GetService<MiniWorker>();
            var bot = prov.GetService<TelegramBotClient>();
            await bot.SetWebhookAsync(String.Empty);

            while (true)
            {
                try
                {
                    await worker.GetMessages();
                    Thread.Sleep(TimeSpan.FromSeconds(1));
                    Console.WriteLine($"{DateTime.Now}:Всё збс");
                }
                catch (Exception e)
                {
                    var color = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"{DateTime.Now}:Message:{e.Message}" +
                                      $"\n InnerException:{e.InnerException}" +
                                      $"\nStackTrace:{e.StackTrace}");
                    Console.ForegroundColor = color;
                }
            }
        }
    }

    public interface ICommand
    {
        string CommandText { get; }

        Task Handler(Message message);
    }

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
            var number = random.Next(0,2);
            var result = number == 1
                ? $"Я тебя тоже люблю, сладенький мой {message.From.FirstName}"
                : $"Вадимка любит только Саню! Так что, {message.From.FirstName} иди нахуй!";
            await _botClient.SendTextMessageAsync(message.Chat.Id, result);
        }
    }
    
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
             
            var strickerId = stickers.Stickers[random.Next(0,stickers.Stickers.Count)].FileId;
            await _botClient.SendStickerAsync(message.Chat.Id, new FileToSend(strickerId));
        }
    }

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

    public class Resolver
    {
        private readonly TelegramBotClient _botClient;
        private readonly IServiceProvider _provider;

        public Resolver(TelegramBotClient botClient, IServiceProvider collection)
        {
            _botClient = botClient;
            _provider = collection;
        }

        public ICommand Resolve(Message message)
        {
            if (message.Type == MessageType.TextMessage)
            {
                if (message.Text == null || !message.Text.StartsWith("/"))
                    return null;
                
                var command = message.Text?.Split("@").FirstOrDefault()?.ToLower();
                var result = _provider.GetServices<ICommand>()
                    .SingleOrDefault(x => x.CommandText?.ToLower() == command);
                if (result == null)
                    return _provider.GetServices<ICommand>()
                        .SingleOrDefault(x => x.CommandText.ToLower() == "defaultcommand");
                return result;
            }

            return _provider.GetServices<ICommand>()
                .Single(x => x.CommandText.ToLower() == "defaultcommand");
        }
    }

    public class MiniWorker
    {
        private readonly TelegramBotClient _botClient;
        private readonly Resolver _resolver;
        private int _offset { get; set; }

        public MiniWorker(TelegramBotClient botClient, Resolver resolver)
        {
            _botClient = botClient;
            _resolver = resolver;
            _offset = 0;
        }

        public async Task GetMessages()
        {
            var updates = await _botClient.GetUpdatesAsync(_offset);

            foreach (var update in updates)
            {
                if (update.Type == UpdateType.MessageUpdate)
                {
                    if (update.Message.Type == MessageType.TextMessage)
                    {
                        var command = _resolver.Resolve(update.Message);
                        if (command != null)
                            await command.Handler(update.Message);
                    }
                }

                _offset = update.Id + 1;
            }
        }
    }
}