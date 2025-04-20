using System.ComponentModel.DataAnnotations;

namespace ISC_ELIB_SERVER.DTOs.Requests
{
    public class SubjectGroupRequest
    {
        [Required(ErrorMessage = "Tên không được để trống")]
        [MaxLength(100, ErrorMessage = "Tên không được vượt quá 100 ký tự")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Id teacher không được để trống")]
        [Range(1, int.MaxValue, ErrorMessage = "{0} phải là số nguyên dương")]
        public int TeacherId { get; set; }
    }
}
