using ISC_ELIB_SERVER.Models;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ISC_ELIB_SERVER.Repositories
{
    public class ClassUserRepo
    {
        private readonly isc_dbContext _context;

        public ClassUserRepo(isc_dbContext context)
        {
            _context = context;
        }

        public async Task<List<ClassUser>> GetAll()
        {
            return await _context.ClassUsers.ToListAsync();
        }

        public async Task<ClassUser> GetById(int id)
        {
            return await _context.ClassUsers
                .Include(cu => cu.User)
                .Include(cu => cu.Class)
                .Include(cu => cu.UserStatus)
                .FirstOrDefaultAsync(cu => cu.Id == id);
        }

        public async Task<List<ClassUser>> GetByClassId(int classId)
        {
            return await _context.ClassUsers
                .Where(cu => cu.ClassId == classId)
                .Include(cu => cu.User)
                .Include(cu => cu.UserStatus)
                .ToListAsync();
        }

        public async Task<bool> Add(ClassUser classUser)
        {
            _context.ClassUsers.Add(classUser);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> Update(ClassUser classUser)
        {
            _context.ClassUsers.Update(classUser);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> Delete(int id)
        {
            var classUser = await _context.ClassUsers.FindAsync(id);
            if (classUser == null) return false;

            _context.ClassUsers.Remove(classUser);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> RemoveByClassId(int classId)
        {
            var classUsers = await _context.ClassUsers.Where(cu => cu.ClassId == classId).ToListAsync();
            if (!classUsers.Any()) return false;

            _context.ClassUsers.RemoveRange(classUsers);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<ClassUser>> GetByCondition(Expression<Func<ClassUser, bool>> predicate)
        {
            return await _context.ClassUsers.Where(predicate).ToListAsync();
        }

    }
}
