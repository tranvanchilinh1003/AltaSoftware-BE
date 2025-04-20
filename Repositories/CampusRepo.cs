using ISC_ELIB_SERVER.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace ISC_ELIB_SERVER.Repositories
{
    public class CampusRepo
    {
        private readonly isc_dbContext _context;
        public CampusRepo(isc_dbContext context)
        {
            _context = context;
        }

        public ICollection<Campus> GetCampuses()
        {
            return _context.Campuses
                .Include(c => c.User)
                .Where(c => c.Active)
                .ToList();
        }

        public Campus GetCampusById(long id)
        {
            return _context.Campuses
                .Include(c => c.User)
                .Where(c => c.Active).FirstOrDefault(c => c.Id == id);
        }

        public Campus CreateCampus(Campus campus)
        {
            _context.Campuses.Add(campus);
            _context.SaveChanges();
            return campus;
        }

        public Campus UpdateCampus(Campus campus)
        {
            _context.Campuses.Update(campus);
            _context.SaveChanges();
            return campus;
        }

        public bool DeleteCampus(long id)
        {
            var campus = GetCampusById(id);
            if (campus != null)
            {
                campus.Active = false;
                _context.Campuses.Update(campus);
                return _context.SaveChanges() > 0;
            }
            return false;
        }
    }
}
