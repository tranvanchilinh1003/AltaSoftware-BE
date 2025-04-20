using System;
using ISC_ELIB_SERVER.Models;
using Microsoft.EntityFrameworkCore;

namespace ISC_ELIB_SERVER.Repositories
{
    public class ExamScheduleClassRepo
    {
        private readonly isc_dbContext _context;

        public ExamScheduleClassRepo(isc_dbContext context)
        {
            _context = context;
        }

        public PagedResult<ExamScheduleClass> GetAll(int page, int pageSize, string? searchTerm, string? sortBy, string? sortOrder)
        {
            var query = _context.ExamScheduleClasses
                .Include(esc => esc.Class)
                .Include(esc => esc.SupervisoryTeacher)
                    .ThenInclude(t => t.User)
                .Include(esc => esc.ExampleScheduleNavigation)
                    .ThenInclude(es => es.Semester)
                .Include(esc => esc.ExampleScheduleNavigation)
                    .ThenInclude(es => es.GradeLevels)
                .Include(esc => esc.ExampleScheduleNavigation)
                    .ThenInclude(es => es.Exam) 
                        .ThenInclude(e => e.ExamGraders)
                            .ThenInclude(eg => eg.User)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(x => x.Class.Name.Contains(searchTerm));
            }

            if (!string.IsNullOrEmpty(sortBy))
            {
                bool isDescending = sortOrder?.ToLower() == "desc";
                query = sortBy.ToLower() switch
                {
                    "id" => isDescending ? query.OrderByDescending(x => x.Id) : query.OrderBy(x => x.Id),
                    _ => query.OrderBy(x => x.Id)
                };
            }

            int totalItems = query.Count();
            var items = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return new PagedResult<ExamScheduleClass>(items, totalItems, page, pageSize);
        }

        //public ExamSchedule? GetDetailWithClasses(long id)
        //{
        //    return _context.ExamSchedules
        //        .Include(e => e.AcademicYear)
        //        .Include(e => e.SubjectNavigation)
        //        .Include(e => e.Semester)
        //        .Include(e => e.GradeLevels)
        //        .Include(e => e.GradeLevels)
        //        .Include(e => e.Exam)
        //            .ThenInclude(ex => ex.ExamGraders)
        //                .ThenInclude(eg => eg.User)
        //        .Include(e => e.ExamScheduleClasses)
        //            .ThenInclude(esc => esc.Class)
        //                .ThenInclude(c => c.ClassUsers)
        //        .FirstOrDefault(e => e.Id == id && e.Active);
        //}
        public ExamScheduleClass? GetById(long id)
        {
            return _context.ExamScheduleClasses.Find(id);
        }

        public void Create(ExamScheduleClass entity)
        {
            _context.ExamScheduleClasses.Add(entity);
            _context.SaveChanges();
        }

        public void Update(ExamScheduleClass entity)
        {
            _context.ExamScheduleClasses.Update(entity);
            _context.SaveChanges();
        }
        public void UpdateStudentCount(int examScheduleId, int classId, int studentCount)
        {
            // Sử dụng thuộc tính phù hợp để lọc đối tượng, ví dụ:
            var examClass = _context.ExamScheduleClasses
                            .FirstOrDefault(x => x.ExampleSchedule == examScheduleId && x.ClassId == classId);
            if (examClass != null)
            {
                // Cập nhật số lượng học sinh tham gia
                examClass.joined_student_quantity = studentCount;
                _context.SaveChanges();
            }
            else
            {
                // Bạn có thể ném exception hoặc xử lý không tìm thấy đối tượng tại đây
                throw new Exception("Không tìm thấy đối tượng ExamScheduleClass với các tham số đã cho");
            }
        }
        public void RemoveClassFromSchedule(int examScheduleId, int classId)
        {
            // Sử dụng thuộc tính phù hợp; nếu model của bạn đang dùng "ExampleSchedule" làm khóa liên kết lịch thi:
            var examClass = _context.ExamScheduleClasses
                            .Include(x => x.ExamGraders)
                            .FirstOrDefault(x => x.ExampleSchedule == examScheduleId && x.ClassId == classId);

            if (examClass != null)
            {
                // Xóa các đối tượng liên quan nếu cần
                _context.ExamGraders.RemoveRange(examClass.ExamGraders);
                // Xóa đối tượng lớp khỏi lịch thi
                _context.ExamScheduleClasses.Remove(examClass);
                _context.SaveChanges();
            }
            else
            {
                throw new Exception("Không tìm thấy đối tượng ExamScheduleClass với các tham số đã cho");
            }
        }
    }
}
