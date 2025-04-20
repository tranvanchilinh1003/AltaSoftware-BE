using System.ComponentModel.DataAnnotations;
using ISC_ELIB_SERVER.Enums;

namespace ISC_ELIB_SERVER.DTOs.Requests
{
    public class SupportRequest
    {
        [Required(ErrorMessage = "Tiêu đề không được để trống")]
        [MaxLength(100, ErrorMessage = "Tiêu đề không được vượt quá 100 ký tự")]
        public string? Title { get; set; }

        [MaxLength(255, ErrorMessage = "Nội dung không được vượt quá 255 ký tự")]
        public string? Content { get; set; }

        [Required(ErrorMessage = "Chọn loại hỗ trợ")]
        public int Type { get; set; }

    }
}
