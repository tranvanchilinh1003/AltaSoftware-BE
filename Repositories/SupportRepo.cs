using ISC_ELIB_SERVER.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace ISC_ELIB_SERVER.Repositories
{
    public class SupportRepo
    {
        private readonly isc_dbContext _context;

        public SupportRepo(isc_dbContext context)
        {
            _context = context;
        }

        public ICollection<Support> GetSupports()
        {
            return _context.Supports
                .Include(s => s.User)
                .ToList();
        }

        public Support GetSupportById(long id)
        {
            return _context.Supports
                .Include(s => s.User)
                .FirstOrDefault(n => n.Id == id);
        }

        public Support CreateSupport(Support support)
        {
            _context.Supports.Add(support);
            _context.SaveChanges();
            return _context.Supports
                .Include(s => s.User)
                .FirstOrDefault(s => s.Id == support.Id);
        }

        public Support UpdateSupport(Support support)
        {
            _context.Supports.Update(support);
            _context.SaveChanges();
            return _context.Supports
                .Include(s => s.User)
                .FirstOrDefault(s => s.Id == support.Id);
        }

        public bool DeleteSupport(long id)
        {
            var Support = GetSupportById(id);
            if (Support != null)
            {
                _context.Supports.Remove(Support);
                return _context.SaveChanges() > 0;
            }
            return false;
        }
    }
}
