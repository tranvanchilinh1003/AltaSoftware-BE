using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
namespace ISC_ELIB_SERVER.DTOs.Requests
{
    public class TestAnswerRequest
    {
         [Required]
        public int QuestionId { get; set; }

        [Required]
        [MaxLength(255, ErrorMessage = "Câu trả lời không được vượt quá 255 ký tự")]
        public string AnswerText { get; set; }

        [Required]
        public bool IsCorrect { get; set; }
    }
}