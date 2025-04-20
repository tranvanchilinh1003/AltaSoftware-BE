using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;
using Microsoft.EntityFrameworkCore;

namespace ISC_ELIB_SERVER.Repositories
{
    public interface ITeachingAssignmentsRepo
    {
        IQueryable<TeachingAssignment> GetTeachingAssignments();
        TeachingAssignment GetTeachingAssignmentById(int id);
        TeachingAssignment CreateTeachingAssignment(TeachingAssignment teachingAssignment);
        TeachingAssignment UpdateTeachingAssignment(TeachingAssignment teachingAssignment);
        bool DeleteTeachingAssignment(List<int> ids);
    }

    public class TeachingAssignmentsRepo : ITeachingAssignmentsRepo
    {
        private readonly isc_dbContext _context;

        public TeachingAssignmentsRepo(isc_dbContext context)
        {
            _context = context;
        }


        public TeachingAssignment GetTeachingAssignmentById(int id)
        {
            return _context.TeachingAssignments
               .Include(t => t.User)
               .Include(t => t.Class)
               .Include(t => t.Subject)
                    .ThenInclude(s => s.SubjectGroup)
               .Include(t => t.Topics)
               .Include(t => t.Sessions)
               .Include(t => t.Semester)
               .ThenInclude(se => se.AcademicYear)
               .FirstOrDefault(t => t.Id == id);
        }

        public TeachingAssignment CreateTeachingAssignment(TeachingAssignment teachingAssignment)
        {
            _context.TeachingAssignments.Add(teachingAssignment);
            _context.SaveChanges();
            return _context.TeachingAssignments
                .Include(ta => ta.User)
                .Include(ta => ta.Class)
                .Include(ta => ta.Subject)
                    .ThenInclude(s => s.SubjectGroup)
                .Include(ta => ta.Topics)
                .Include(ta => ta.Sessions)
                .Include(ta => ta.Semester)
                    .ThenInclude(s => s.AcademicYear)
                .FirstOrDefault(ta => ta.Id == teachingAssignment.Id);
            }

        public TeachingAssignment? UpdateTeachingAssignment(TeachingAssignment teachingAssignment)
        {
            var existingAssignment = _context.TeachingAssignments
                .Include(t => t.User)
                .Include(t => t.Class)
                .Include(t => t.Subject)
                .Include(t => t.Topics)
                .Include(t => t.Semester)
                .FirstOrDefault(t => t.Id == teachingAssignment.Id);

            if (existingAssignment == null)
            {
                return null;
            }

            existingAssignment.UserId = teachingAssignment.UserId;
            existingAssignment.ClassId = teachingAssignment.ClassId;
            existingAssignment.SubjectId = teachingAssignment.SubjectId;
            existingAssignment.TopicsId = teachingAssignment.TopicsId;
            existingAssignment.SemesterId = teachingAssignment.SemesterId;
            existingAssignment.StartDate = teachingAssignment.StartDate;
            existingAssignment.EndDate = teachingAssignment.EndDate;
            existingAssignment.Description = teachingAssignment.Description;

            _context.SaveChanges();

            return existingAssignment;
        }


        public bool DeleteTeachingAssignment(List<int> ids)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var teachingAssignments = _context.TeachingAssignments.Where(t => ids.Contains(t.Id)).ToList();
                if (!teachingAssignments.Any()) return false;

                foreach (var item in teachingAssignments)
                {
                    item.Active = false;
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

        public IQueryable<TeachingAssignment> GetTeachingAssignments()
        {
            return _context.TeachingAssignments
                .Include(t => t.User)
                .Include(t => t.Class)
                .Include(t => t.Subject)
                .Include(t => t.Topics)
                .Include(t => t.Sessions)
                .Include(t => t.Semester);
        }
    }
}
