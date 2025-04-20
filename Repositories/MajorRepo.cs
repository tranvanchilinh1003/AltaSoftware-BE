using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Repositories
{
    public class MajorRepo
    {
        private readonly isc_dbContext _context;
        public MajorRepo(isc_dbContext context)
        {
            _context = context;
        }

        public ICollection<Major> GetMajor()
        {
            return _context.Majors.ToList();
        }

        public Major GetMajorById(long id)
        {
            return _context.Majors.FirstOrDefault(s => s.Id == id);
        }

        public Major CreateMajor(Major major)
        {
            _context.Majors.Add(major);
            _context.SaveChanges();
            return major;
        }

        public Major UpdateMajor(Major major)
        {
            _context.Majors.Update(major);
            _context.SaveChanges();
            return major;
        }

        public bool DeleteMajor(Major major)
        {
            _context.Majors.Update(major);
            _context.SaveChanges();
            return true;
        }
    }
}
