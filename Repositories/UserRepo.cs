using ISC_ELIB_SERVER.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace ISC_ELIB_SERVER.Repositories
{
    public class UserRepo
    {
        private readonly isc_dbContext _context;

        public UserRepo(isc_dbContext context)
        {
            _context = context;
        }

        // Lấy tất cả các User
        public ICollection<User> GetUsers()
        {
            return _context.Users.Where(u => u.Active).ToList();
        }

        public int GetQuantityUserByRoleId(int roleId)
        {
            var users = _context.Users.Where(a => a.RoleId == roleId).ToList();

            return users.Count;
        }

        // Lấy User theo Id
        public User GetUserById(int id)
        {
            return _context.Users.FirstOrDefault(u => u.Id == id && u.Active);
        }

        // Lấy User theo Code
        public User GetUserByCode(string code)
        {
            return _context.Users.FirstOrDefault(u => u.Code == code && u.Active);
        }

        // Tạo mới một User
        public User CreateUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return user;
        }

        // Cập nhật thông tin User
        public User UpdateUser(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
            return user;
        }

        // Xóa User
        public bool DeleteUser(int id)
        {
            var user = GetUserById(id);
            if (user != null)
            {
                user.Active = false;
                return _context.SaveChanges() > 0;
            }
            return false;
        }

        // Lấy danh sách User theo id lớp học
        public async Task<ICollection<User>> GetUsersByClassId(int classId, string roleName = "Student")
        {
            return await _context.Users
            .Where(u => u.ClassId == classId && u.Active && u.Role.Name == roleName)
            .Include(r => r.Role)
            .ToListAsync();
        }

        // Lấy danh sách User theo id, id lớp học và năm học
        public async Task<User> GetStudentById(int userId, string roleName = "Student")
        {
            return await _context.Users
           .Where(u => u.Id == userId &&
                       u.Active &&
                       u.Role.Name == roleName)
           .Include(u => u.Role)
           .Include(u => u.AcademicYear)
           .Include(u => u.Class)
               .ThenInclude(c => c.GradeLevel)
           .Include(u => u.Class)
               .ThenInclude(c => c.ClassType)
           .Include(u => u.Class)
               .ThenInclude(c => c.User)
           .FirstOrDefaultAsync();
        }

        // Lấy danh sách User theo id lớp học và năm học
        public async Task<ICollection<User>> GetUsersByClassIdAndAcademicYearId(int classId, int academicYearId, string roleName = "Student")
        {
            return await _context.Users
           .Where(u => u.ClassId == classId &&
                       u.AcademicYearId == academicYearId &&
                       u.Active &&
                       u.Role.Name == roleName)
           .Include(r => r.Role)
           .ToListAsync();
        }
    }
}
