using ISC_ELIB_SERVER.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ISC_ELIB_SERVER.Repositories
{
    public interface IStudentScoreRepo
    {
        ICollection<StudentScore> GetStudentScores();
        StudentScore GetStudentScoreById(int id);
        StudentScore CreateStudentScore(StudentScore studentScore);
        StudentScore UpdateStudentScore(StudentScore studentScore);
        bool DeleteStudentScore(int id);
        ICollection<StudentScore> GetStudentScoresByUserIdAndSubjectIdAndSemesterId(int userId, int subjectId, int semesterId);

    }

    public class StudentScoreRepo : IStudentScoreRepo
    {
        private readonly isc_dbContext _context;

        public StudentScoreRepo(isc_dbContext context)
        {
            _context = context;
        }

        public ICollection<StudentScore> GetStudentScores()
        {
            return _context.StudentScores.ToList();
        }

        public StudentScore GetStudentScoreById(int id)
        {
            return _context.StudentScores.FirstOrDefault(s => s.Id == id);
        }

        public StudentScore CreateStudentScore(StudentScore studentScore)
        {
            _context.StudentScores.Add(studentScore);
            _context.SaveChanges();
            return studentScore;
        }

        public StudentScore? UpdateStudentScore(StudentScore studentScore)
        {
            var existingStudentScore = _context.StudentScores.Find(studentScore.Id);

            if (existingStudentScore == null)
            {
                return null;
            }

            existingStudentScore.UserId = studentScore.UserId;
            existingStudentScore.ScoreTypeId = studentScore.ScoreTypeId;
            existingStudentScore.SubjectId = studentScore.SubjectId;
            existingStudentScore.SemesterId = studentScore.SemesterId;

            _context.SaveChanges();
            return existingStudentScore;
        }

        public bool DeleteStudentScore(int id)
        {
            var studentScore = _context.StudentScores.Find(id);

            if (studentScore == null)
            {
                return false;
            }

            _context.StudentScores.Remove(studentScore);
            return _context.SaveChanges() > 0;
        }

        public ICollection<StudentScore> GetStudentScoresByUserIdAndSubjectIdAndSemesterId(int userId, int subjectId, int semesterId)
        {
            return _context.StudentScores
            .Include(s => s.ScoreType)
            .Where(s => s.UserId == userId && s.SubjectId == subjectId && s.SemesterId == semesterId)
                .ToList();
        }
    }
}
