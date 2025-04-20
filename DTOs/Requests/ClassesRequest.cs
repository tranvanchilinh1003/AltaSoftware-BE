using System.ComponentModel.DataAnnotations;

namespace ISC_ELIB_SERVER.DTOs.Requests
{
    public class ClassesRequest
    {

        [MaxLength(50, ErrorMessage = "Mã không được vượt quá 50 ký tự")]
        public string? Code { get; set; }

        [Required(ErrorMessage = "Tên không được để trống")]
        [MaxLength(100, ErrorMessage = "Tên không được vượt quá 100 ký tự")]
        public string? Name { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Số lượng học sinh phải lớn hơn 0")]
        public int? StudentQuantity { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Số lượng môn học phải lớn hơn 0")]
        public int? SubjectQuantity { get; set; }

        [MaxLength(255, ErrorMessage = "Mô tả không được vượt quá 255 ký tự")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Cấp lớp không được để trống")]
        public int? GradeLevelId { get; set; }

        [Required(ErrorMessage = "Năm học không được để trống")]
        public int? AcademicYearId { get; set; }
        public int? UserId { get; set; }

        [Required(ErrorMessage = "Loại lớp không được để trống")]
        public int? ClassTypeId { get; set; }

        public List<int> Subjects { get; set; } = new List<int>();

    }
}
