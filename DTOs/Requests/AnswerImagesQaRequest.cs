using System.ComponentModel.DataAnnotations;

namespace ISC_ELIB_SERVER.DTOs.Requests
{
    public class AnswerImagesQaRequest
    {
        [Required(ErrorMessage = "AnswerId là bắt buộc")]
        public int AnswerId { get; set; }

        [Required(ErrorMessage = "ImageUrl là bắt buộc")]
        [Url(ErrorMessage = "Định dạng URL không hợp lệ")]
        public string ImageUrl { get; set; } = string.Empty;
    }
}
