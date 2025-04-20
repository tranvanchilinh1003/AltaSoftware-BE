using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ISC_ELIB_SERVER.Models;
using Microsoft.EntityFrameworkCore;

namespace ISC_ELIB_SERVER.Repositories
{
    public interface IClassSubjectRepo
    {
        Task<List<ClassSubject>> GetClassSubjectsByClassIdAsync(int classId);
        Task<bool> AddClassSubjectsAsync(int classId, List<int> subjectIds);
        Task<bool> RemoveClassSubjectsByClassIdAsync(int classId, List<int> subjectIds);
        Task<bool> RemoveClassSubjectsByClassIdAsync(int classId); // Sửa trả về Task<bool>
        Task AddClassSubjectsAsync(List<ClassSubject> classSubjects);
        Task<ICollection<ClassSubject>> GetClassSubjectsByClassId(int classId);
        Task<ClassSubject> GetClassSubjectByClassIdAndSubjectId(int classId, int subjectId);
    }

    public class ClassSubjectRepo : IClassSubjectRepo
    {
        private readonly isc_dbContext _context;

        public ClassSubjectRepo(isc_dbContext context)
        {
            _context = context;
        }

        public async Task<List<ClassSubject>> GetClassSubjectsByClassIdAsync(int classId)
        {
            return await _context.ClassSubjects
                .Where(cs => cs.ClassId == classId)
                .Include(cs => cs.Subject)
                .ToListAsync();
        }

        public async Task<bool> AddClassSubjectsAsync(int classId, List<int> subjectIds)
        {
            try
            {
                var newClassSubjects = subjectIds.Select(subjectId => new ClassSubject
                {
                    ClassId = classId,
                    SubjectId = subjectId
                }).ToList();

                await _context.ClassSubjects.AddRangeAsync(newClassSubjects);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi thêm môn học vào lớp: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> RemoveClassSubjectsByClassIdAsync(int classId, List<int> subjectIds)
        {
            try
            {
                var classSubjects = await _context.ClassSubjects
                    .Where(cs => cs.ClassId == classId && subjectIds.Contains(cs.Id))
                    .ToListAsync();

                if (classSubjects.Any())
                {
                    _context.ClassSubjects.RemoveRange(classSubjects);
                    await _context.SaveChangesAsync();
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi xóa môn học khỏi lớp: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> RemoveClassSubjectsByClassIdAsync(int classId)
        {
            try
            {
                var classSubjects = await _context.ClassSubjects
                    .Where(cs => cs.ClassId == classId)
                    .ToListAsync();

                if (classSubjects.Any())
                {
                    _context.ClassSubjects.RemoveRange(classSubjects);
                    await _context.SaveChangesAsync();
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi xóa tất cả môn học của lớp: {ex.Message}");
                return false;
            }
        }

        public async Task AddClassSubjectsAsync(List<ClassSubject> classSubjects)
        {
            if (classSubjects == null || !classSubjects.Any())
                throw new ArgumentException("Danh sách môn học không được rỗng");

            await _context.ClassSubjects.AddRangeAsync(classSubjects);
            await _context.SaveChangesAsync();
        }

        public async Task<ICollection<ClassSubject>> GetClassSubjectsByClassId(int classId)
        {
            return await _context.ClassSubjects
                .Where(cs => cs.ClassId == classId)
                .Include(cs => cs.Subject)
                .ToListAsync();
        }

        public async Task<ClassSubject> GetClassSubjectByClassIdAndSubjectId(int classId, int subjectId)
        {
            return await _context.ClassSubjects
                .Where(cs => cs.ClassId == classId && cs.SubjectId == subjectId)
                .Include(cs => cs.Subject)
                .FirstOrDefaultAsync();
        }
    }
}
