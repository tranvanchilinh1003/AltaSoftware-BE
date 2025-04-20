using System.ComponentModel.DataAnnotations;

namespace ISC_ELIB_SERVER.DTOs.Requests
{
    public class QuestionImagesQaRequest
    {
        [Required(ErrorMessage = "QuestionId là bắt buộc")]
        public int QuestionId { get; set; }

        [Required(ErrorMessage = "ImageUrl là bắt buộc")]
        [Url(ErrorMessage = "Định dạng URL không hợp lệ")]
        public string ImageUrl { get; set; } = string.Empty;
    }
}
