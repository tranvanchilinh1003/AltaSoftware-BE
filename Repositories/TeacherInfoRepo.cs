using ISC_ELIB_SERVER.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace ISC_ELIB_SERVER.Repositories
{
    public class TeacherInfoRepo
    {
        private readonly isc_dbContext _context;

        // Constructor nhận context
        public TeacherInfoRepo(isc_dbContext context)
        {
            _context = context;
        }

        // Create: Thêm mới TeacherInfo vào database
        public TeacherInfo CreateTeacherInfo(TeacherInfo teacherInfo)
        {
            _context.TeacherInfos.Add(teacherInfo);
            _context.SaveChanges();
            return teacherInfo;
        }

        // Read: Lấy tất cả TeacherInfos từ database
        public List<TeacherInfo> GetAllTeacherInfo()
        {
            return _context.TeacherInfos.Where(t => t.Active).ToList();
        }

        // Read: Lấy TeacherInfo theo Id
        public TeacherInfo GetTeacherInfoById(int id)
        {
            return _context.TeacherInfos.FirstOrDefault(t => t.Id == id && t.Active);
        }

        // Update: Cập nhật thông tin TeacherInfo
        public void UpdateTeacherInfo(TeacherInfo teacherInfo)
        {
            _context.TeacherInfos.Update(teacherInfo);
            _context.SaveChanges();
        }

        // Delete: Xóa TeacherInfo theo Id
        public void DeleteTeacherInfo(int id)
        {
            var teacherInfo = _context.TeacherInfos.FirstOrDefault(t => t.Id == id);
            if (teacherInfo != null)
            {
                teacherInfo.Active = false; // Xóa mềm
                _context.SaveChanges();
            }
        }
    }
}
