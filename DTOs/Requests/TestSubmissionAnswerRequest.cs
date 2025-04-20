using System.ComponentModel.DataAnnotations;

namespace ISC_ELIB_SERVER.DTOs.Requests
{
    public class TestSubmissionAnswerRequest
    {
        //[Required(ErrorMessage = "Mã số nộp bài không được bỏ trống")]
        //[Range(1, int.MaxValue, ErrorMessage = "Mã số nộp bài phải lớn hơn 0")]
        public int? SubmissionId { get; set; }

        [Required(ErrorMessage = "Mã câu hỏi không được bỏ trống")]
        [Range(1, int.MaxValue, ErrorMessage = "Mã câu hỏi phải lớn hơn 0")]
        public int QuestionId { get; set; }

        // Optional: vì có thể là tự luận
        [Range(1, int.MaxValue, ErrorMessage = "Mã câu trả lời phải lớn hơn 0")]
        public int? SelectedAnswerId { get; set; }

        // Tự luận có thể rỗng, nhưng bạn vẫn có thể kiểm tra độ dài tối đa nếu muốn
        [MaxLength(5000, ErrorMessage = "Câu trả lời không được vượt quá 5000 ký tự")]
        public string? AnswerText { get; set; }

        public bool Active { get; set; } = true;

        [Range(0, 10, ErrorMessage = "Điểm phải từ 0 đến 10")]
        public double? Score { get; set; }

        [MaxLength(1000, ErrorMessage = "Ghi chú giáo viên không được vượt quá 1000 ký tự")]
        public string? TeacherComment { get; set; }

        // Nếu cần file đính kèm:
        // public List<TestSubmissionAnswerAttachmentRequest>? Attachments { get; set; }
        public List<IFormFile>? Attachments { get; set; }
    }
}
