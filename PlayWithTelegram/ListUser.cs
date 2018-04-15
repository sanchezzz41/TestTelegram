using System.Collections.Generic;
using System.Linq;

namespace PlayWithTelegram
{
    public class ListUser
    {
        private readonly List<User> _users;

        public ListUser()
        {
            _users = new List<User>();
        }

        public bool AddUserId(long chatId, int userId)
        {
            if (_users.Any(x => x.ChatId == chatId && x.UserId == userId))
                return false;
            var result = new User(chatId,userId);
            _users.Add(result);
            return true;
        }

        public List<User> GetAll()
        {
            return new List<User>(_users);
        }

        public List<User> GetUsersByTheChatId(long chatId)
        {
            return new List<User>(_users.Where(x => x.ChatId == chatId));
        }
    }

    public class User
    {
        public long ChatId { get; set; }

        public int UserId { get; set; }

        public User(long chatId, int userId)
        {
            ChatId = chatId;
            UserId = userId;
        }
    }
}