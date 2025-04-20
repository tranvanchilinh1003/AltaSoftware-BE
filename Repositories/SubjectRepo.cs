using ISC_ELIB_SERVER.Models;
using Microsoft.EntityFrameworkCore;

namespace ISC_ELIB_SERVER.Repositories
{
    public class SubjectRepo
    {
        private readonly isc_dbContext _context;

        public SubjectRepo(isc_dbContext context)
        {
            _context = context;
        }

        public IQueryable<Subject> GetAllSubject()
        {
            return _context.Subjects
                            .Include(s => s.SubjectGroup)
                            .Include(s => s.SubjectType);
        }

        public Subject GetSubjectById(long id)
        {
            return _context.Subjects
                            .Include(s => s.SubjectGroup)
                            .Include(s => s.SubjectType)
                            .FirstOrDefault(x => x.Id == id);
        }

        public Subject CreateSubject(Subject Subject)
        {
            _context.Subjects.Add(Subject);
            _context.SaveChanges();
            return Subject;
        }

        public Subject UpdateSubject(Subject Subject)
        {
            _context.Subjects.Update(Subject);
            _context.SaveChanges();
            return Subject;
        }

        public bool DeleteSubject(long id)
        {
            var subject = GetSubjectById(id);
            if (subject != null)
            {
                subject.Active = false;
                _context.Subjects.Update(subject);
                return _context.SaveChanges() > 0;
            }
            return false;
        }
    }
}
