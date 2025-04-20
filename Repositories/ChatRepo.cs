using ISC_ELIB_SERVER.Models;
using System;

namespace ISC_ELIB_SERVER.Repositories
{
    public class ChatRepo
    {
        private readonly isc_dbContext _context;

        public ChatRepo(isc_dbContext context)
        {
            _context = context;
        }

        public IEnumerable<Chat> GetChatsBySessionId(long sessionId)
        {
            return _context.Chats.Where(c => c.SessionId == sessionId).ToList();
        }

        public Chat? GetChatById(long id)
        {
            return _context.Chats.FirstOrDefault(c => c.Id == id);
        }

        public Chat CreateChat(Chat chat)
        {
            _context.Chats.Add(chat);
            _context.SaveChanges();
            return chat;
        }

        public Chat UpdateChat(Chat chat)
        {
            _context.Chats.Update(chat);
            _context.SaveChanges();
            return chat;
        }

        public bool DeleteChat(long id)
        {
            var chat = _context.Chats.FirstOrDefault(c => c.Id == id);
            if (chat == null) return false;

            _context.Chats.Remove(chat);
            _context.SaveChanges();
            return true;
        }
    }
}
