using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Repositories
{
    public class ExamRepo
    {
        private readonly isc_dbContext _context;

        public ExamRepo(isc_dbContext context)
        {
            _context = context;
        }

        public ICollection<Exam> GetExams()
        {
            return _context.Exams.ToList();
        }
        public Exam GetExamById(long id)
        {
            return _context.Exams.FirstOrDefault(e => e.Id == id);
        }
        public ICollection<Exam> GetExamByName(string name)
        {
            return _context.Exams
                .Where(e => e.Name.Contains(name))
                .ToList();
        }
        public Exam CreateExam(Exam exam)
        {
            _context.Exams.Add(exam);
            _context.SaveChanges();
            return exam;
        }
        public Exam UpdateExam(Exam exam)
        {
            _context.Exams.Update(exam);
            _context.SaveChanges();
            return exam;
        }
        public bool DeleteExam(long id)
        {
            var exam = GetExamById(id);
            if (exam != null)
            {
                exam.Active = !exam.Active;
                return _context.SaveChanges() > 0;
            }
            return false;
        }
        public void Detach<T>(T entity) where T : class
        {
            _context.Entry(entity).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
        }

    }
}
