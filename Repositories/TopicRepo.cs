using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Repositories
{
    public class TopicRepo
    {
        private readonly isc_dbContext _context;
        public TopicRepo(isc_dbContext context)
        {
            _context = context;
        }

        public ICollection<Topic> GetAllTopics()
        {
            return _context.Topics.ToList();
        }

        public Topic GetTopicById(int id)
        {
            return _context.Topics.FirstOrDefault(x => x.Id == id);
        }

        public Topic CreateTopic(Topic topic)
        {
            _context.Topics.Add(topic);
            _context.SaveChanges();
            return topic;
        }

        public bool DeleteTopic(int id)
        {
            var topic = GetTopicById(id);
            if (topic != null)
            {
                _context.Topics.Remove(topic);
                return _context.SaveChanges() > 0;
            }
            return false;
        }

        public Topic UpdateTopic(Topic topic)
        {
            _context.Topics.Update(topic);
            _context.SaveChanges();
            return topic;
        }

    }
}