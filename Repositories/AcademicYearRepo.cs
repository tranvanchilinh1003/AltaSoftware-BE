using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace ISC_ELIB_SERVER.Repositories
{
    public class AcademicYearRepo
    {
        private readonly isc_dbContext _context;
        public AcademicYearRepo(isc_dbContext context)
        {
            _context = context;
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }

        public ICollection<AcademicYear> GetAcademicYears()
        {
            return _context.AcademicYears
                .Where(a => a.Active)
                .Include(a => a.Semesters)
                .Where(a => a.Active)
                .ToList()
                .Select(a => new AcademicYear
                {
                    Id = a.Id,
                    StartTime = a.StartTime,
                    EndTime = a.EndTime,
                    Semesters = a.Semesters.Where(s => s.Active).ToList()
                })
                .ToList();
        }

        public AcademicYear GetAcademicYearById(long id)
        {
            return _context.AcademicYears
                .Include(a => a.Semesters)
                .Where(a => a.Active)
                .Select(a => new AcademicYear
                {
                    Id = a.Id,
                    StartTime = a.StartTime,
                    EndTime = a.EndTime,
                    Semesters = a.Semesters.Where(s => s.Active).ToList()
                })
                .FirstOrDefault(s => s.Id == id);
        }

        public bool IsDuplicateAcademicYear(long schoolId, DateTime startTime, DateTime endTime, long? excludeId = null)
        {
            return _context.AcademicYears.Any(a =>
                a.SchoolId == schoolId &&
                a.Active &&
                a.StartTime == startTime &&
                a.EndTime == endTime &&
                (!excludeId.HasValue || a.Id != excludeId.Value));
        }

        public AcademicYear CreateAcademicYear(AcademicYear academicYear)
        {
            if (IsDuplicateAcademicYear(academicYear.SchoolId ?? 0,
             academicYear.StartTime ?? DateTime.Now, academicYear.EndTime ?? DateTime.Now))
            {
                throw new Exception("Niên khóa này đã tồn tại trong trường.");
            }

            _context.AcademicYears.Add(academicYear);
            _context.SaveChanges();
            return academicYear;
        }

        public AcademicYear UpdateAcademicYear(AcademicYear academicYear)
        {
            if (IsDuplicateAcademicYear(academicYear.SchoolId ?? 0,
             academicYear.StartTime ?? DateTime.Now, academicYear.EndTime ?? DateTime.Now))
            {
                throw new Exception("Niên khóa này đã tồn tại trong trường.");
            }

            _context.AcademicYears.Update(academicYear);
            _context.SaveChanges();
            return academicYear;
        }

        public bool DeleteAcademicYear(long id)
        {
            var academicYear = GetAcademicYearById(id);
            if (academicYear != null)
            {
                academicYear.Active = false;
                _context.AcademicYears.Update(academicYear);
                return _context.SaveChanges() > 0;
            }
            return false;
        }

        public ICollection<AcademicYear> GetAcademicYearsBySchoolId(long schoolId)
        {
            return _context.AcademicYears
                .Where(a => a.SchoolId == schoolId && a.Active)
                .ToList();
        }
    }
}
