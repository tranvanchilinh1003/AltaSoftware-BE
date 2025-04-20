using ISC_ELIB_SERVER.Models;
using Microsoft.EntityFrameworkCore;

namespace ISC_ELIB_SERVER.Repositories
{
    public class EducationLevelRepo
    {
        private readonly isc_dbContext _context;
        public EducationLevelRepo(isc_dbContext context)
        {
            _context = context;
        }

        public ICollection<EducationLevel> GetEducationLevels()
        {
            return _context.EducationLevels
                .Where(a => a.Active)
                .ToList();
        }

        public EducationLevel GetEducationLevelById(long id)
        {
            return _context.EducationLevels.Where(a => a.Active).FirstOrDefault(s => s.Id == id);
        }

        public EducationLevel CreateEducationLevel(EducationLevel EducationLevel)
        {
            _context.ChangeTracker.Clear();
            _context.EducationLevels.Add(EducationLevel);
            _context.SaveChanges();
            return EducationLevel;
        }

        public EducationLevel UpdateEducationLevel(EducationLevel EducationLevel)
        {
            _context.EducationLevels.Update(EducationLevel);
            _context.SaveChanges();
            return EducationLevel;
        }

        public bool DeleteEducationLevel(long id)
        {
            var EducationLevel = GetEducationLevelById(id);
            if (EducationLevel != null)
            {
                EducationLevel.Active = false;
                _context.EducationLevels.Update(EducationLevel);
                return _context.SaveChanges() > 0;
            }
            return false;
        }
    }
}