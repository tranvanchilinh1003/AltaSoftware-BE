using ISC_ELIB_SERVER.Models;
using Microsoft.EntityFrameworkCore;

namespace ISC_ELIB_SERVER.Repositories
{
    public class ExamScheduleRepo
    {
        private readonly isc_dbContext _context;

        public ExamScheduleRepo(isc_dbContext context)
        {
            _context = context;
        }

        public PagedResult<ExamSchedule> GetAll(
     int page,
     int pageSize,
     string? search,
     string? sortBy,
     bool isDescending,
     int? academicYearId,
     int? semesterId,
     int? gradeLevelsId, 
     int? classId         
 )
        {
            var query = _context.ExamSchedules
      .Include(e => e.AcademicYear)
      .Include(e => e.SubjectNavigation)
      .Include(e => e.Semester)
      .Include(e => e.GradeLevels)
     .Include(e => e.ExamScheduleClasses)
        .ThenInclude(esc => esc.Class)
            .ThenInclude(c => c.User)
    .Include(e => e.ExamScheduleClasses)
        .ThenInclude(esc => esc.ExamGraders)
            .ThenInclude(eg => eg.User)
      .Where(e => e.Active) 
      .AsNoTracking();
            if (academicYearId.HasValue)
            {
                query = query.Where(e => e.AcademicYear.Id == academicYearId.Value);
            }

            if (semesterId.HasValue)
            {
                query = query.Where(e => e.Semester.Id == semesterId.Value);
            }

            if (gradeLevelsId.HasValue)
            {
                query = query.Where(e => e.GradeLevels.Id == gradeLevelsId.Value);
            }

            if (classId.HasValue)
            {
                query = query.Where(e => e.ExamScheduleClasses.Any(c => c.ClassId == classId.Value));
            }

           
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(e => e.Name.ToLower().Contains(search.ToLower()));
            }

          
            if (!string.IsNullOrEmpty(sortBy))
            {
                query = isDescending
                    ? query.OrderByDescending(e => EF.Property<object>(e, sortBy))
                    : query.OrderBy(e => EF.Property<object>(e, sortBy));
            }

            
            var totalCount = query.Count();

      
            var items = query.Skip((page - 1) * pageSize)
                             .Take(pageSize)
                             .ToList();

            return new PagedResult<ExamSchedule>(items, totalCount, page, pageSize);
        }
        public ExamSchedule? GetById(long id)
        {
            var examSchedule = _context.ExamSchedules
                .Include(e => e.AcademicYear)
                .Include(e => e.SubjectNavigation)
                .Include(e => e.Semester)
                .Include(e => e.GradeLevels)
                .Include(e => e.Exam)
                    .ThenInclude(ex => ex.ExamGraders)
                        .ThenInclude(eg => eg.User)
                // Thêm include ExamScheduleClasses để lấy thông tin lớp và liên quan
                .Include(e => e.ExamScheduleClasses)
                    .ThenInclude(esc => esc.Class)
                        .ThenInclude(c => c.User)
                          .Include(e => e.ExamScheduleClasses)
            .ThenInclude(esc => esc.Class)
                // Nếu cần include ExamGraders cho từng lớp
                .Include(e => e.ExamScheduleClasses)
                    .ThenInclude(esc => esc.ExamGraders)
                        .ThenInclude(eg => eg.User)
                .AsNoTracking()
                .FirstOrDefault(e => e.Id == id && e.Active);
            return examSchedule;
        }
        public ExamSchedule? GetDetailWithClasses(long id)
        {
            return _context.ExamSchedules
                .Include(es => es.AcademicYear)
                .Include(es => es.SubjectNavigation)
                .Include(es => es.Semester)
                .Include(es => es.GradeLevels)
                .Include(es => es.Exam)
                    .ThenInclude(e => e.ExamGraders)
                        .ThenInclude(eg => eg.User)                       // graders chung (nếu cần)
                .Include(es => es.ExamScheduleClasses)
                    .ThenInclude(esc => esc.Class)
                        .ThenInclude(c => c.User)                        // ← giáo viên chủ nhiệm (Class.User)
                .Include(es => es.ExamScheduleClasses)
                    .ThenInclude(esc => esc.ExamGraders)
                        .ThenInclude(eg => eg.User)                       // ← graders riêng theo lớp
                .AsNoTracking()
                .FirstOrDefault(es => es.Id == id && es.Active);
        }
        public List<ExamSchedule> GetForCalendar(
    int academicYearId, int? semesterId, int? gradeLevelsId, int? classId)
        {
            var query = _context.ExamSchedules
                .Include(e => e.AcademicYear)
                .Include(e => e.SubjectNavigation)
                .Include(e => e.Semester)
                .Include(e => e.GradeLevels)
                 .Include(e => e.Exam)
                    .ThenInclude(ex => ex.ExamGraders)
                        .ThenInclude(eg => eg.User)
                            .Include(e => e.ExamScheduleClasses)
        .ThenInclude(esc => esc.Class)
            .ThenInclude(c => c.User)
    .Include(e => e.ExamScheduleClasses)
        .ThenInclude(esc => esc.ExamGraders)
            .ThenInclude(eg => eg.User)
                .Where(e => e.Active && e.AcademicYear.Id == academicYearId);

            if (semesterId.HasValue)
                query = query.Where(e => e.Semester.Id == semesterId.Value);
            if (gradeLevelsId.HasValue)
                query = query.Where(e => e.GradeLevels.Id == gradeLevelsId.Value);
            if (classId.HasValue)
                query = query.Where(e => e.ExamScheduleClasses.Any(c => c.ClassId == classId.Value));

            return query.AsNoTracking().ToList();
        }
        public void Create(ExamSchedule examSchedule)
        {
            _context.ExamSchedules.Add(examSchedule);
            _context.SaveChanges();
        }
        public void AddGraders(int examId, int examScheduleClassId, List<int> graderIds)
        {
            if (graderIds == null || !graderIds.Any()) return;

            var existingGraderIds = _context.ExamGraders
                .Where(g => g.ExamId == examId && g.ExamScheduleClassId == examScheduleClassId)
                .Select(g => g.UserId)
                .ToHashSet();

            var newGraders = graderIds
                .Distinct()
                .Where(id => !existingGraderIds.Contains(id))
                .Select(id => new ExamGrader
                {
                    ExamId = examId,
                    ExamScheduleClassId = examScheduleClassId,
                    UserId = id,
                    Active = true
                })
                .ToList();

            if (newGraders.Any())
            {
                _context.ExamGraders.AddRange(newGraders);
                _context.SaveChanges();
            }
        }

        public bool Exists(int academicYearId, int semesterId, int gradeLevelsId, DateTime examDay, int classId)
        {
            return _context.ExamSchedules
                .Include(e => e.ExamScheduleClasses)
                .Any(e =>
                    e.Active
                    && e.AcademicYearId == academicYearId
                    && e.SemesterId == semesterId
                    && e.GradeLevelsId == gradeLevelsId
                    && e.ExamDay.HasValue && e.ExamDay.Value.Date == examDay.Date
                    && e.ExamScheduleClasses.Any(c => c.ClassId == classId && c.Active)
                );
        }
        public void Update(ExamSchedule examSchedule)
        {
            _context.ExamSchedules.Update(examSchedule);
            _context.SaveChanges();
        }

        public ExamSchedule? GetByIdForUpdate(long id)
        {
            // Không sử dụng AsNoTracking để EF tự động theo dõi các thay đổi của entity
            return _context.ExamSchedules
                .Include(e => e.AcademicYear)
                .Include(e => e.SubjectNavigation)
                .Include(e => e.Semester)
                .Include(e => e.GradeLevels)
                .Include(e => e.Exam)
                    .ThenInclude(ex => ex.ExamGraders)
                        .ThenInclude(eg => eg.User)
                .FirstOrDefault(e => e.Id == id && e.Active);
        }

        public void RemoveAllClassesAndGraders(long examScheduleId)
        {
            var examSchedule = _context.ExamSchedules
                .Include(x => x.ExamScheduleClasses)
                .ThenInclude(c => c.ExamGraders)
                .FirstOrDefault(x => x.Id == examScheduleId);

            if (examSchedule != null)
            {
                foreach (var examClass in examSchedule.ExamScheduleClasses)
                {
                    _context.ExamGraders.RemoveRange(examClass.ExamGraders);
                }

                _context.ExamScheduleClasses.RemoveRange(examSchedule.ExamScheduleClasses);
                _context.SaveChanges();
            }
        }
        public bool Delete(int id)
        {
            var entity = _context.ExamSchedules.Find(id);
            if (entity == null) return false;

            
            entity.Active = false;
            _context.SaveChanges();
            return true;
        }
    }
}
