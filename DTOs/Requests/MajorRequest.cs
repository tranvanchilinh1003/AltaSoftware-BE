using System.ComponentModel.DataAnnotations;

namespace ISC_ELIB_SERVER.DTOs.Requests
{
    public class MajorRequest
    {
        [Required(ErrorMessage = "Tên chuyên ngành không được để trống")]
        [MaxLength(100, ErrorMessage = "Tên chuyên ngành không được vượt quá 100 ký tự")]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500, ErrorMessage = "Mô tả không được vượt quá 500 ký tự")]
        public string Description { get; set; } = string.Empty;
    }
}
