using System;
using ISC_ELIB_SERVER.Models;
using Microsoft.EntityFrameworkCore;

namespace ISC_ELIB_SERVER.Repositories
{
    public class SubjectTypeRepo
    {
        private readonly isc_dbContext _context;
        public SubjectTypeRepo(isc_dbContext context)
        {
            _context = context;
        }

        public ICollection<SubjectType> GetAllSubjectType()
        {
            return _context.SubjectTypes.Include(st => st.AcademicYear).ToList();
        }

        public SubjectType GetSubjectTypeById(long id)
        {
            return _context.SubjectTypes.Include(st => st.AcademicYear).FirstOrDefault(x => x.Id == id);
        }

        public SubjectType CreateSubjectType(SubjectType subjectType)
        {
            _context.SubjectTypes.Add(subjectType);
            _context.SaveChanges();
            return subjectType;
        }

        public SubjectType UpdateSubjectType(SubjectType subjectType)
        {
            _context.SubjectTypes.Update(subjectType);
            _context.SaveChanges();
            return subjectType;
        }

        public bool DeleteSubjectType(long id)
        {
            var subjectType = GetSubjectTypeById(id);
            if (subjectType != null)
            {
                subjectType.Active = false;
                _context.SubjectTypes.Update(subjectType);
                return _context.SaveChanges() > 0;
            }
            return false;
        }
    }
}
