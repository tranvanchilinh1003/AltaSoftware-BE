using ISC_ELIB_SERVER.Models;
using Microsoft.EntityFrameworkCore;

namespace ISC_ELIB_SERVER.Repositories
{
    public class SchoolRepo
    {
        private readonly isc_dbContext _context;
        public SchoolRepo(isc_dbContext context)
        {
            _context = context;
        }

        public ICollection<School> GetSchools()
        {
            return _context.Schools
                .Include(s => s.User)
                .Include(s => s.EducationLevel)
                .Include(s => s.Campuses)
                .ThenInclude(s => s.User)
                .Include(s => s.AcademicYears)
                .ThenInclude(s => s.Semesters)
                .Where(s => s.Active)
                .ToList();
        }

        public School GetSchoolById(long id)
        {
            return _context.Schools
                .Include(s => s.User)
                .Include(s => s.EducationLevel)
                .Include(s => s.Campuses)
                .ThenInclude(s => s.User)
                .Include(s => s.AcademicYears)
                .ThenInclude(s => s.Semesters)
                .Where(s => s.Active)
                .FirstOrDefault(s => s.Id == id);
        }

        public School CreateSchool(School school)
        {
            _context.Schools.Add(school);
            _context.SaveChanges();
            return school;
        }

        public School UpdateSchool(School school)
        {
            _context.Schools.Update(school);
            _context.SaveChanges();
            return school;
        }

        public bool DeleteSchool(long id)
        {
            var school = GetSchoolById(id);
            if (school != null)
            {
                school.Active = false;
                _context.Schools.Update(school);
                return _context.SaveChanges() > 0;
            }
            return false;
        }


        public bool IsSchoolNameExists(string? name)
        {
            return _context.Schools.Any(s => s.Name == name);
        }


        public bool IsSchoolNameExists(string? name, long id)
        {
            return _context.Schools.Any(s => s.Name == name && s.Id != id);
        }
    }
}
