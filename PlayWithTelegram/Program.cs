using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using PlayWithTelegram.Commands;
using Telegram.Bot;

namespace PlayWithTelegram
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();

            var botClient = new TelegramBotClient("553480406:AAETczW5qnJp4ZkbQ2_E77l2oyzP3IF9qig");
            var cashe = new ListUser();
            serviceCollection.AddScoped<ICommand, HelloCommand>();
            serviceCollection.AddScoped<ICommand, FuckYouCommand>();
            serviceCollection.AddScoped<ICommand, DefaultCommand>();
            serviceCollection.AddScoped<ICommand, GetRandomStickerCommand>();
            serviceCollection.AddScoped<ICommand, MiniGameCommand>();
            serviceCollection.AddScoped<ICommand, RegisterInTheGameCommand>();
            serviceCollection.AddScoped<ICommand, ListUsersInTheGameCommand>();
            serviceCollection.AddSingleton(botClient);
            serviceCollection.AddSingleton<Resolver>();
            serviceCollection.AddSingleton<MiniWorker>();
            serviceCollection.AddSingleton(cashe);
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

    #region Commands

    #endregion
}