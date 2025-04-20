using ISC_ELIB_SERVER.Models;
using System;

namespace ISC_ELIB_SERVER.Repositories
{
    public class DiscussionRepo
    {
        private readonly isc_dbContext _context;

        public DiscussionRepo(isc_dbContext context)
        {
            _context = context;

        }

        public ICollection<Discussion> GetDiscussions()
        {
            return _context.Discussions.ToList();
        }

        public Discussion GetDiscussionById(long id)
        {
            return _context.Discussions.FirstOrDefault(s => s.Id == id);
        }
        public Discussion CreateDiscussion(Discussion discussion)
        {
            _context.Discussions.Add(discussion);
            _context.SaveChanges();
            return discussion;
        }
        public Discussion UpdateDiscussion(Discussion discussion)
        {
            _context.Discussions.Update(discussion);
            _context.SaveChanges();
            return discussion;
        }

        public bool DeleteDiscussion(long id)
        {
            var discussion = GetDiscussionById(id);
            if (discussion != null)
            {
                _context.Discussions.Remove(discussion);
                return _context.SaveChanges() > 0;
            }
            return false;
        }
    }

}
