using System.ComponentModel.DataAnnotations;

namespace ISC_ELIB_SERVER.DTOs.Requests
{
    public class DiscussionRequest
    {
        [Required(ErrorMessage = "TopicId không được để trống")]
        public int TopicId { get; set; }

        [Required(ErrorMessage = "UserId không được để trống")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Nội dung không được để trống")]
        public string Content { get; set; } = string.Empty;
    }
}
