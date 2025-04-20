using System.ComponentModel.DataAnnotations;

namespace ISC_ELIB_SERVER.DTOs.Requests
{
    public class NotificationRequest
    {
        [Required(ErrorMessage = "Tiêu đề không được để trống")]
        [MaxLength(100, ErrorMessage = "Tiêu đề không được vượt quá 100 ký tự")]
        public string Title { get; set; } = string.Empty;
        [Required(ErrorMessage = "Nội dung không được để trống")]
        [MaxLength(255, ErrorMessage = "Nội dung không được vượt quá 255 ký tự")]
        public string Content { get; set; } = string.Empty;

        [Required(ErrorMessage = "Loại thông báo không được để trống")]
        [MaxLength(50, ErrorMessage = "Loại thông báo không được vượt quá 50 ký tự")]
        public string Type { get; set; } = string.Empty;

        //[Required(ErrorMessage = "Mã người gửi không được để trống")]
        //public int SenderId { get; set; }

        [Required(ErrorMessage = "Mã người nhận không được để trống")]
        public int UserId { get; set; }
    }
}
