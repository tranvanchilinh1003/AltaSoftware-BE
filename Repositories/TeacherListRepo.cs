using ISC_ELIB_SERVER.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace ISC_ELIB_SERVER.Repositories
{
    public class TeacherListRepo
    {
        private readonly isc_dbContext _context;

        public TeacherListRepo(isc_dbContext context)
        {
            _context = context;
        }

        // Lấy tất cả TeacherInfo với các Include cần thiết
        public IQueryable<TeacherInfo> GetAllTeacherList()
        {
            return _context.TeacherInfos
                .Include(t => t.User)
                .ThenInclude(t => t.SubjectGroups)
                .Include(t => t.Retirements)
                .Where(t => t.Active == true && t.User.Active == true);
        }

        // Lấy TeacherInfo theo Id
        public TeacherInfo? GetTeacherListById(int id)
        {
            return _context.TeacherInfos
                .Include(t => t.User)
                .ThenInclude(t => t.SubjectGroups)
                .Include(t => t.Retirements)
                .Where(t => t.Active == true && t.User.Active == true)
                .FirstOrDefault(t => t.Id == id);
        }
    }
}
