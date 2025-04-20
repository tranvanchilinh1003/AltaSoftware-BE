using System.ComponentModel.DataAnnotations;

namespace ISC_ELIB_SERVER.DTOs.Requests
{
    public class QuestionQaRequest
    {
        [Required(ErrorMessage = "Nội dung câu hỏi không được để trống")]
        [MaxLength(1000, ErrorMessage = "Nội dung câu hỏi không được vượt quá 1000 ký tự")]
        public string Content { get; set; } = string.Empty;
        [Required]
        public int SubjectId { get; set; }
        [Required]
        public int ClassId { get; set; }

    }
}
