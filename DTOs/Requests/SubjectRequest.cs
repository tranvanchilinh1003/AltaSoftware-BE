using System.ComponentModel.DataAnnotations;

namespace ISC_ELIB_SERVER.DTOs.Requests
{
    public class SubjectRequest
    {
        [Required(ErrorMessage = "Mã không được để trống")]
        public string Code { get; set; }
        [Required(ErrorMessage = "Tên không được để trống")]
        [MaxLength(100, ErrorMessage = "Tên không được vượt quá 100 ký tự")]
        public string Name { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "{0} phải là số nguyên dương")]
        public int HoursSemester1 { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "{0} phải là số nguyên dương")]
        public int HoursSemester2 { get; set; }
        [Required(ErrorMessage = "SubjectGroupId không được để trống")]
        [Range(1, int.MaxValue, ErrorMessage = "{0} phải là số nguyên dương")]
        public int SubjectGroupId { get; set; }
        [Required(ErrorMessage = "SubjectTypeId không được để trống")]
        [Range(1, int.MaxValue, ErrorMessage = "{0} phải là số nguyên dương")]
        public int SubjectTypeId { get; set; }
    }
}
