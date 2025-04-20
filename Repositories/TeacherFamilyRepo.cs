using ISC_ELIB_SERVER.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace ISC_ELIB_SERVER.Repositories
{
    public class TeacherFamilyRepo
    {
        private readonly isc_dbContext _context;

        public TeacherFamilyRepo(isc_dbContext context)
        {
            _context = context;
        }

        public ICollection<TeacherFamily> GetTeacherFamilies()
        {
            return _context.TeacherFamilies
                .Where(tf => tf.Active) // Chỉ lấy những bản ghi đang hoạt động
                .ToList();
        }

        public TeacherFamily? GetTeacherFamilyById(long id)
        {
            return _context.TeacherFamilies
                .AsNoTracking()
                .FirstOrDefault(tf => tf.Id == id && tf.Active);
        }

        public TeacherFamily? CreateTeacherFamily(TeacherFamily teacherFamily)
        {
            // Kiểm tra teacher_id có tồn tại trong TeacherInfo
            bool teacherExists = _context.TeacherInfos.Any(t => t.Id == teacherFamily.TeacherId);
            if (!teacherExists)
            {
                throw new Exception($"TeacherId {teacherFamily.TeacherId} không tồn tại.");
            }

            _context.TeacherFamilies.Add(teacherFamily);
            _context.SaveChanges();
            return teacherFamily;
        }


        public TeacherFamily? UpdateTeacherFamily(TeacherFamily teacherFamily)
        {
            var existing = _context.TeacherFamilies.FirstOrDefault(tf => tf.Id == teacherFamily.Id && tf.Active);
            if (existing == null) return null;

            _context.Entry(existing).CurrentValues.SetValues(teacherFamily);
            _context.SaveChanges();
            return teacherFamily;
        }

        public bool DeleteTeacherFamily(long id)
        {
            var teacherFamily = _context.TeacherFamilies.FirstOrDefault(tf => tf.Id == id && tf.Active);
            if (teacherFamily == null) return false;

            teacherFamily.Active = false; // Xóa mềm bằng cách đặt Active = false
            _context.TeacherFamilies.Update(teacherFamily);
            return _context.SaveChanges() > 0;
        }
    }
}
