using System.ComponentModel.DataAnnotations;

namespace ISC_ELIB_SERVER.DTOs.Requests
{
    public class AnswersQaRequest
    {
        [Required(ErrorMessage = "Nội dung câu trả lời không được để trống")]
        [MaxLength(1000, ErrorMessage = "Nội dung câu trả lời không được vượt quá 1000 ký tự")]
        public string Content { get; set; } = string.Empty;

        [Required(ErrorMessage = "QuestionId là bắt buộc")]
        public int QuestionId { get; set; }

       public List<IFormFile>? Files { get; set; }
    }
}
