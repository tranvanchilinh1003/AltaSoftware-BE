using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;
using Microsoft.EntityFrameworkCore;

namespace ISC_ELIB_SERVER.Repositories
{
    public class GradeLevelRepo
    {
        private readonly isc_dbContext _context;
        public GradeLevelRepo(isc_dbContext context)
        {
            _context = context;
        }

        public ICollection<GradeLevel> GetGradeLevels()
        {
            return _context.GradeLevels
                .Where(a => a.Active)
                .ToList();
        }

        public ICollection<GradeLevel> GetGradeLevels(string schoolName, int? startYear, int? endYear)
        {
            return (from gl in _context.GradeLevels
                    join ti in _context.TeacherInfos on gl.TeacherId equals ti.Id
                    join u in _context.Users on ti.UserId equals u.Id
                    join ay in _context.AcademicYears on u.AcademicYearId equals ay.Id
                    join sc in _context.Schools on ay.SchoolId equals sc.Id
                    where sc.Name.ToLower() == schoolName.ToLower()
                    && (!startYear.HasValue || ay.StartTime.HasValue && ay.StartTime.Value.Year == startYear.Value)
                    && (!endYear.HasValue || ay.EndTime.HasValue && ay.EndTime.Value.Year == endYear.Value)
                    select new GradeLevel
                    {
                        Id = gl.Id,
                        Code = gl.Code,
                        Name = gl.Name,
                        TeacherId = gl.TeacherId,
                        Active = gl.Active,
                    })
                    .Where(a => a.Active)
                   .ToList();
        }


        public GradeLevel GetGradeLevelById(long id)
        {
            return _context.GradeLevels.Where(a => a.Active).FirstOrDefault(s => s.Id == id);
        }

        public object GetClassesByGradeLevel(long gradeLevelId)
        {
            var result = _context.GradeLevels
                .Where(gl => gl.Active && gl.Id == gradeLevelId) 
                .Select(gl => new
                {
                    gl.Id,
                    gl.Code,
                    gl.Name,
                    gl.TeacherId,
                    Classes = _context.Classes
                        .Where(c => c.Active && c.GradeLevelId == gl.Id) 
                        .Select(c => new
                        {
                            c.Id,
                            c.Code,
                            c.Name,
                            c.StudentQuantity,
                            c.SubjectQuantity,
                            c.Description
                        }).ToList()
                })
                .FirstOrDefault(); 

            return result;
        }



        public GradeLevel CreateGradeLevel(GradeLevel GradeLevel)
        {
            _context.GradeLevels.Add(GradeLevel);
            _context.SaveChanges();
            return GradeLevel;
        }

        public GradeLevel UpdateGradeLevel(GradeLevel GradeLevel)
        {
            _context.GradeLevels.Update(GradeLevel);
            _context.SaveChanges();
            return GradeLevel;
        }

        public bool DeleteGradeLevel(long id)
        {
            var GradeLevel = GetGradeLevelById(id);
            if (GradeLevel != null)
            {
                GradeLevel.Active = false;
                _context.GradeLevels.Update(GradeLevel);
                return _context.SaveChanges() > 0;
            }
            return false;
        }
    }
}