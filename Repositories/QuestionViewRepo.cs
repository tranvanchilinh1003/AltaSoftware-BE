using ISC_ELIB_SERVER.Models;
using Microsoft.EntityFrameworkCore;

namespace ISC_ELIB_SERVER.Repositories
{
    public class QuestionViewRepo
    {
        private readonly isc_dbContext _context;

        public QuestionViewRepo(isc_dbContext context)
        {
            _context = context;
        }

        // Lấy số lượt xem của một câu hỏi
        public int GetViewCount(int questionId)
{
    return _context.QuestionViews
        .Where(q => q.QuestionId == questionId)
        .Count();
}


        // Kiểm tra xem người dùng đã xem câu hỏi này chưa
        public bool HasUserViewed(int questionId, int userId)
        {
            return _context.QuestionViews.Any(q => q.QuestionId == questionId && q.UserId == userId);
        }

        // Thêm lượt xem mới nếu người dùng chưa xem
     public void AddView(int questionId, int userId)
{
    if (!HasUserViewed(questionId, userId))
    {
        var view = new QuestionView 
        { 
            QuestionId = questionId, 
            UserId = userId, 
            ViewedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified) 
        };

        _context.QuestionViews.Add(view);
        _context.SaveChanges();

        // Console.WriteLine($"✅ Đã thêm lượt xem: {questionId} - User {userId}");
    }
    // else
    // {
    //     Console.WriteLine($"⚠️ User {userId} đã xem câu hỏi {questionId} trước đó!");
    // }
}


    }
}
