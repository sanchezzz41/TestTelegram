using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace PlayWithTelegram
{
    public interface ICommand
    {
        string CommandText { get; }

        Task Handler(Message message);
    }
}