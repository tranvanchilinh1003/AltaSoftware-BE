using ISC_ELIB_SERVER.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ISC_ELIB_SERVER.Repositories
{
    public class DiscussionImageRepo
    {
        private readonly isc_dbContext _context;

        public DiscussionImageRepo(isc_dbContext context)
        {
            _context = context;
        }

        public List<DiscussionImage> GetDiscussionImagesByDiscussionId(long discussionId)
        {
            return _context.DiscussionImages.Where(di => di.DiscussionId == discussionId).ToList();
        }

        public DiscussionImage? GetDiscussionImageById(long id)
        {
            return _context.DiscussionImages.FirstOrDefault(di => di.Id == id);
        }

        public DiscussionImage CreateDiscussionImage(DiscussionImage discussionImage)
        {
            _context.DiscussionImages.Add(discussionImage);
            _context.SaveChanges();
            return discussionImage;
        }
        public DiscussionImage UpdateDiscussionImage(DiscussionImage discussionImage)
        {
            _context.DiscussionImages.Update(discussionImage);
            _context.SaveChanges();
            return discussionImage;
        }
        public bool DeleteDiscussionImage(long id)
        {
            var image = _context.DiscussionImages.FirstOrDefault(di => di.Id == id);
            if (image == null) return false;

            _context.DiscussionImages.Remove(image);
            _context.SaveChanges();
            return true;
        }
    }
}
