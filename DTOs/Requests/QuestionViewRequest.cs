using System.ComponentModel.DataAnnotations;

namespace ISC_ELIB_SERVER.DTOs.Requests
{
    public class QuestionViewRequest
    {
        [Required]
        public int QuestionId { get; set; }
        
    }
}
