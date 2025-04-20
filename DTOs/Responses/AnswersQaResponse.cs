namespace ISC_ELIB_SERVER.DTOs.Responses
{
    public class AnswersQaResponse
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreateAt { get; set; }
        public int UserId { get; set; }
        public int QuestionId { get; set; }

        //Thêm thông tin người trả lời
        public string? UserAvatar { get; set; } // Avatar người trả lời
        public string? UserName { get; set; } // Tên người trả lời
        public string? UserRole { get; set; } // Vai trò người trả lời (VD: Giáo viên, Học sinh)
        public List<string>? ImageUrls { get; set; } = new List<string>();
    }
}
