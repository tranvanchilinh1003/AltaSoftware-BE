using System.ComponentModel.DataAnnotations;

namespace ISC_ELIB_SERVER.DTOs.Requests
{
    public class GradeLevelRequest
    {
        [Required(ErrorMessage = "Mã khoa - khối không được để trống")]
        [MaxLength(50, ErrorMessage = "Mã khoa - khối không được vượt quá 50 ký tự")]
        public string Code { get; set; } = string.Empty;

        [Required(ErrorMessage = "Tên khoa - khối không được để trống")]
        [MaxLength(50, ErrorMessage = "Tên khoa - khối không được vượt quá 50 ký tự")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Trưởng khoa/khối không được để trống")]
        [Range(1, int.MaxValue, ErrorMessage = "ID trưởng khoa/khối không hợp lệ")]
        public int? TeacherId { get; set; }
    }
}
