using System.ComponentModel.DataAnnotations;

namespace ISC_ELIB_SERVER.DTOs.Requests
{
    public class ChatRequest
    {
        [Required(ErrorMessage = "Nội dung không được để trống")]
        public string Content { get; set; } = string.Empty;

        [Required(ErrorMessage = "UserId không được để trống")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "SessionId không được để trống")]
        public int SessionId { get; set; }
    }
}
