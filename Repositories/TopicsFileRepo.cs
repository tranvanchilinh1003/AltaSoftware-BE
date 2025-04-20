using System.Collections.Generic;
using System.Linq;
using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Repositories
{
    public interface ITopicsFileRepo
    {
        IEnumerable<TopicsFile> GetAll();
        TopicsFile? GetById(int id);
        TopicsFile Create(TopicsFile topicsFile);
        TopicsFile Update(TopicsFile topicsFile);
        bool Delete(int id);
    }

    public class TopicsFileRepo : ITopicsFileRepo
    {
        private readonly isc_dbContext _context;

        public TopicsFileRepo(isc_dbContext context)
        {
            _context = context;
        }

        public IEnumerable<TopicsFile> GetAll()
        {
            return _context.TopicsFiles.ToList();
        }

        public TopicsFile? GetById(int id)
        {
            return _context.TopicsFiles.Find(id);
        }

        public TopicsFile Create(TopicsFile topicsFile)
        {
            _context.TopicsFiles.Add(topicsFile);
            _context.SaveChanges();
            return topicsFile;
        }

        public TopicsFile Update(TopicsFile topicsFile)
        {
            _context.TopicsFiles.Update(topicsFile);
            _context.SaveChanges();
            return topicsFile;
        }

        public bool Delete(int id)
        {
            var topicsFile = _context.TopicsFiles.Find(id);
            if (topicsFile == null) return false;

            _context.TopicsFiles.Remove(topicsFile);
            _context.SaveChanges();
            return true;
        }
    }
}
