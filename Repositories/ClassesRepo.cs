using ISC_ELIB_SERVER.Models;
using Microsoft.EntityFrameworkCore;
namespace ISC_ELIB_SERVER.Repositories
{
    public interface IClassesRepo
    {
        IQueryable<Class> GetClass();
        Class GetClassById(int id);
        Class CreateClass(Class classes);
        Class? UpdateClass(Class classes);
        bool DeleteClasses(List<int> ids);
        Task<IDisposable> BeginTransactionAsync();
        Task<Class> CreateClassAsync(Class classEntity);
        Task<Class> UpdateClassAsync(Class classEntity);
        Task SaveChangesAsync();

        IQueryable<Subject> GetSubjects();
        Task AddRangeAsync(List<Class> newClasses);
    }

    public class ClassRepo : IClassesRepo
    {
        private readonly isc_dbContext _context;

        public ClassRepo(isc_dbContext context)
        {
            _context = context;
        }

        public IQueryable<Class> GetClass()
        {
            return _context.Classes
                .AsNoTracking()
                .Include(c => c.GradeLevel)
                    .ThenInclude(g => g.Teacher)
                .Include(c => c.AcademicYear)
                    .ThenInclude(a => a.School)
                .Include(c => c.User)
                    .ThenInclude(u => u.Role)
                .Include(c => c.User)
                    .ThenInclude(u => u.AcademicYear)
                .Include(c => c.User)
                    .ThenInclude(u => u.Class)
                .Include(c => c.ClassSubjects)
                    .ThenInclude(s => s.Subject)
                .Include(c => c.ClassType);
        }




        public Class? GetClassById(int id)
        {
            return _context.Classes
                .Include(c => c.GradeLevel)
                    .ThenInclude(g => g.Teacher)
                .Include(c => c.AcademicYear)
                    .ThenInclude(a => a.School)
                .Include(c => c.User)
                    .ThenInclude(u => u.Role)
                .Include(c => c.User)
                    .ThenInclude(u => u.AcademicYear)
                .Include(c => c.User)
                    .ThenInclude(u => u.Class)
                .Include(c => c.ClassType)
                .Include(c => c.ClassSubjects)
                .FirstOrDefault(c => c.Id == id);
        }

        public Class CreateClass(Class newClass)
        {
            _context.Classes.Add(newClass);
            _context.SaveChanges();
            return _context.Classes
                .Include(c => c.GradeLevel)
                    .ThenInclude(g => g.Teacher)
                .Include(c => c.AcademicYear)
                    .ThenInclude(a => a.School)
                .Include(c => c.User)
                    .ThenInclude(u => u.Role)
                .Include(c => c.User)
                    .ThenInclude(u => u.AcademicYear)
                .Include(c => c.User)
                    .ThenInclude(u => u.Class)
                .Include(c => c.ClassType)
                 .Include(c => c.ClassSubjects)
                    .ThenInclude(cs => cs.Subject)
                .FirstOrDefault(c => c.Id == newClass.Id);
        }

        public Class? UpdateClass(Class updatedClass)
        {
            var existingClass = _context.Classes
                .Include(c => c.GradeLevel)
                    .ThenInclude(g => g.Teacher)
                .Include(c => c.AcademicYear)
                    .ThenInclude(a => a.School)
                .Include(c => c.User)
                    .ThenInclude(u => u.Role)
                .Include(c => c.User)
                    .ThenInclude(u => u.AcademicYear)
                .Include(c => c.User)
                    .ThenInclude(u => u.Class)
                .Include(c => c.ClassType)
                .Include(c => c.ClassSubjects)
                    .ThenInclude(cs => cs.Subject)
                .FirstOrDefault(c => c.Id == updatedClass.Id);

            if (existingClass == null)
            {
                return null;
            }

            existingClass.Name = updatedClass.Name;
            existingClass.Description = updatedClass.Description;
            existingClass.StudentQuantity = updatedClass.StudentQuantity;
            existingClass.SubjectQuantity = updatedClass.SubjectQuantity;
            existingClass.GradeLevelId = updatedClass.GradeLevelId;
            existingClass.AcademicYearId = updatedClass.AcademicYearId;
           
            _context.SaveChanges();

            return _context.Classes
                .Include(c => c.GradeLevel)
                .Include(c => c.AcademicYear)
                .Include(c => c.User)
                .Include(c => c.ClassType)
                .Include(c => c.ClassSubjects)
                    .ThenInclude(cs => cs.Subject)
                .FirstOrDefault(c => c.Id == updatedClass.Id);
        }


        public bool DeleteClasses(List<int> ids)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var classes = _context.Classes.Where(c => ids.Contains(c.Id)).ToList();
                if (!classes.Any()) return false;

                foreach (var classItem in classes)
                {
                    classItem.Active = false;
                }

                _context.SaveChanges();
                transaction.Commit();
                return true;
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }


        public async Task<Class> CreateClassAsync(Class classEntity)
        {
            await _context.Classes.AddAsync(classEntity);
            await _context.SaveChangesAsync();

            return await _context.Classes

                .Include(c => c.GradeLevel)
                .Include(c => c.AcademicYear)
                .Include(c => c.User)
                .Include(c => c.ClassType)
                .Include(c => c.ClassSubjects)
                    .ThenInclude(cs => cs.Subject)
                .FirstOrDefaultAsync(c => c.Id == classEntity.Id);
        }

        public async Task<Class?> UpdateClassAsync(Class classEntity)
        {
            var existingClass = await _context.Classes.FindAsync(classEntity.Id);
            if (existingClass == null) return null;

            _context.Entry(existingClass).CurrentValues.SetValues(classEntity);
            existingClass.Active = true;
            await _context.SaveChangesAsync();

            return await _context.Classes
                .Include(c => c.GradeLevel)
                .Include(c => c.AcademicYear)
                .Include(c => c.User)
                .Include(c => c.ClassType)
                .Include(c => c.ClassSubjects)
                    .ThenInclude(cs => cs.Subject)
                .FirstOrDefaultAsync(c => c.Id == classEntity.Id);
        }

        async Task<IDisposable> IClassesRepo.BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public IQueryable<Subject> GetSubjects()
        {
            return _context.Subjects.AsQueryable();
        }

        public async Task AddRangeAsync(List<Class> newClasses)
        {
            if (newClasses == null || !newClasses.Any())
                return; 

            await _context.Classes.AddRangeAsync(newClasses);
            await _context.SaveChangesAsync();
        }

    }
}
