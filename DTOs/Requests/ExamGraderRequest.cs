using System.ComponentModel.DataAnnotations;

namespace ISC_ELIB_SERVER.DTOs.Requests
{
    public class ExamGraderRequest
    {
        [Required]
        public int ExamId { get; set; }

        [Required]
        public int UserId { get; set; }

        public string? ClassIds { get; set; }
    }
}
