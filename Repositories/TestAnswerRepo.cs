using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Repositories
{
    public class TestAnswerRepo
    {
        private readonly isc_dbContext _context;
        public TestAnswerRepo(isc_dbContext context)
        {
            _context = context;
        }

        public bool IsCorrectAnswer(long answerId)
        {
            return _context.TestAnswers.Any(a => a.Id == answerId && a.IsCorrect == true);
        }
          public List<TestAnswer> GetAnswersByQuestion(int questionId)
        {
            return _context.TestAnswers
                .Where(t => t.QuestionId == questionId)
                .ToList();
        }

        //  Lấy câu trả lời theo ID
        public TestAnswer? GetAnswerById(int id)
        {
            return _context.TestAnswers.Find(id);
        }

        //  Tạo câu trả lời
        public TestAnswer CreateTestAnswer(TestAnswer testAnswer)
    {
        _context.TestAnswers.Add(testAnswer);
        _context.SaveChanges();
        return testAnswer;
    }

        //  Cập nhật câu trả lời
        public TestAnswer UpdateAnswer(TestAnswer answer)
        {
            _context.TestAnswers.Update(answer);
            _context.SaveChanges();
            return answer;
        }

        //  Xóa câu trả lời
        public bool DeleteAnswer(int id)
        {
            var answer = _context.TestAnswers.Find(id);
            if (answer == null) return false;

            _context.TestAnswers.Remove(answer);
            _context.SaveChanges();
            return true;
        }
    }
}
