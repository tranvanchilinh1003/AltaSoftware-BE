using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Services;
using Microsoft.EntityFrameworkCore;

namespace ISC_ELIB_SERVER.Repositories
{
    public class QuestionQaRepo
    {
        private readonly isc_dbContext _context;
       

        public QuestionQaRepo(isc_dbContext context)
        {
            _context = context;
           
        }

     public ICollection<QuestionQa> GetQuestions(int? classId, int? subjectId)
        {
            var query = _context.QuestionQas
                .Include(q => q.User) // Lấy thông tin người đặt câu hỏi
                .Include(q => q.Subject)
                .Include(q => q.AnswersQas)
                .Include(q => q.QuestionImagesQas)
                .Where(q => _context.TeachingAssignments
                    .Any(t => t.SubjectId == subjectId && t.ClassId == classId && t.Active && t.SubjectId == q.SubjectId))
                .OrderByDescending(q => q.CreateAt) //  Lấy câu hỏi mới nhất trước
                .ToList();

            return query;
        }

         public ICollection<QuestionQa> GetAnsweredQuestions(int? classId, int? subjectId)
        {
            var query = _context.QuestionQas
                .Include(q => q.User) // Lấy thông tin người đặt câu hỏi
                .Include(q => q.Subject)
                .Include(q => q.AnswersQas) // Lấy câu trả lời
                .Include(q => q.QuestionImagesQas)
                .Where(q => q.AnswersQas.Any() && // Chỉ lấy câu hỏi có câu trả lời
                    _context.TeachingAssignments
                        .Any(t => t.SubjectId == subjectId && t.ClassId == classId && t.Active && t.SubjectId == q.SubjectId))
                .OrderByDescending(q => q.CreateAt) //  Sắp xếp theo ngày mới nhất
                .ToList();

            return query;
        }

         public ICollection<QuestionQa> GetRecentQuestions(int? classId, int? subjectId)
        {
            var query = _context.QuestionQas
                .Include(q => q.User) // Lấy thông tin người đặt câu hỏi
                .Include(q => q.Subject)
                .Include(q => q.AnswersQas)
                .Include(q => q.QuestionImagesQas)
                .Where(q =>
                    _context.TeachingAssignments
                        .Any(t => t.SubjectId == subjectId && t.ClassId == classId && t.Active && t.SubjectId == q.SubjectId))
                .OrderByDescending(q => q.CreateAt) // 🏆 Sắp xếp mới nhất
                .ToList();

            return query;
        }



        public QuestionQa GetQuestionById(long id)
        {
            return _context.QuestionQas
                .Include(q => q.User)
                .Include(q => q.Subject)
                .Include(q => q.AnswersQas)
                .Include(q => q.QuestionImagesQas)
                .FirstOrDefault(q => q.Id == id);
        }

    public QuestionQa CreateQuestion(QuestionQa question, List<string>? imageBase64s)
{
    _context.QuestionQas.Add(question);
    _context.SaveChanges();

    //  Lưu danh sách ảnh Base64
    if (imageBase64s != null && imageBase64s.Count > 0)
    {
        foreach (var base64String in imageBase64s)
        {
            var image = new QuestionImagesQa
            {
                QuestionId = question.Id,
                ImageUrl = base64String, // Lưu nguyên vẹn Base64 vào DB
                Active = true
            };
            _context.QuestionImagesQas.Add(image);
        }
        _context.SaveChanges();
    }

    return question;
}
        public QuestionQa UpdateQuestion(QuestionQa question)
        {
            _context.QuestionQas.Update(question);
            _context.SaveChanges();
            return question;
        }

      public void DeleteQuestionImages(long questionId)
        {
            var images = _context.QuestionImagesQas.Where(i => i.QuestionId == questionId).ToList();
            if (images.Any())
            {
                _context.QuestionImagesQas.RemoveRange(images);
                _context.SaveChanges();
            }
        }
        public bool DeleteQuestion(long id)
        {
            var question = GetQuestionById(id);
            if (question == null)
            {
                return false; // Không tìm thấy câu hỏi
            }

            //  Xóa tất cả hình ảnh của câu hỏi
            var questionImages = _context.QuestionImagesQas.Where(qi => qi.QuestionId == id).ToList();
            _context.QuestionImagesQas.RemoveRange(questionImages);

            // Lấy danh sách câu trả lời liên quan
            var answers = _context.AnswersQas.Where(a => a.QuestionId == id).ToList();

            foreach (var answer in answers)
            {
                // Xóa tất cả hình ảnh của câu trả lời
                var answerImages = _context.AnswerImagesQas.Where(ai => ai.AnswerId == answer.Id).ToList();
                _context.AnswerImagesQas.RemoveRange(answerImages);
            }

            // Xóa tất cả câu trả lời của câu hỏi
            _context.AnswersQas.RemoveRange(answers);

            //Cuối cùng, xóa câu hỏi
            _context.QuestionQas.Remove(question);

            _context.SaveChanges();
            return true;
        }

        
        public ICollection<QuestionQa> SearchQuestionsByUserName(string userName, bool onlyAnswered, int? classId, int? subjectId)
        {
            var query = _context.QuestionQas
                .Include(q => q.User)
                .Include(q => q.Subject)
                .Include(q => q.AnswersQas)
                .Include(q => q.QuestionImagesQas)
                .Where(q =>
                    q.User != null &&
                    q.User.FullName.ToLower().Contains(userName.ToLower()) && // 🔍 Tìm theo tên
                    _context.TeachingAssignments.Any(t =>
                        t.SubjectId == subjectId && 
                        t.ClassId == classId && 
                        t.Active &&
                        t.SubjectId == q.SubjectId) // ✅ Kiểm tra lớp + môn học
                );

            if (onlyAnswered) 
            {
                query = query.Where(q => q.AnswersQas.Any()); // 🔥 Chỉ lấy câu hỏi đã trả lời
            }

            return query.OrderByDescending(q => q.CreateAt).ToList();
        }
        
    }
}
