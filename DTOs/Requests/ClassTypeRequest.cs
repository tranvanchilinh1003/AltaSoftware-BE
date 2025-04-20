using System.ComponentModel.DataAnnotations;
using ISC_ELIB_SERVER.DTOs.Responses;

namespace ISC_ELIB_SERVER.DTOs.Requests
{
    public class ClassTypeRequest
    {
        [Required(ErrorMessage = "Tên không được để trống")]
        [MaxLength(100, ErrorMessage = "Tên không được vượt quá 100 ký tự")]
        public string Name { get; set; } = string.Empty;

        [MaxLength(255, ErrorMessage = "Mô tả không được vượt quá 255 ký tự")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Niên khóa không được để trống")]
        public int? AcademicYearId { get; set; }

        public bool Status { get; set; }
    }
}
