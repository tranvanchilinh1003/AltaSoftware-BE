using System.ComponentModel.DataAnnotations;

namespace ISC_ELIB_SERVER.DTOs.Requests
{
    public class Exemption_AddRequest
    {
        [Required(ErrorMessage = "Mã sinh viên không được để trống.")]
        [Range(1, int.MaxValue, ErrorMessage = "Mã sinh viên phải lớn hơn 0.")]
        public int StudentId { get; set; }

        [Required(ErrorMessage = "Mã lớp không được để trống.")]
        [Range(1, int.MaxValue, ErrorMessage = "Mã lớp phải lớn hơn 0.")]
        public int ClassId { get; set; }

        [Required(ErrorMessage = "Đối tượng miễn giảm không được để trống.")]
        [StringLength(500, ErrorMessage = "Đối tượng miễn giảm không được vượt quá 500 ký tự.")]
        public string? ExemptedObjects { get; set; }

        [Required(ErrorMessage = "Hình thức miễn giảm không được để trống.")]
        [StringLength(255, ErrorMessage = "Hình thức miễn giảm không được vượt quá 255 ký tự.")]
        public string? FormExemption { get; set; }
    }
}
