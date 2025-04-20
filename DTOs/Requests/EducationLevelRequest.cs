using System.ComponentModel.DataAnnotations;

namespace ISC_ELIB_SERVER.DTOs.Requests
{
    public class EducationLevelRequest
    {
        [Required(ErrorMessage = "Tên bậc học - trình độ không được để trống")]
        [MaxLength(100, ErrorMessage = "Tên bậc học - trình độ không được vượt quá 100 ký tự")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Hình thức đào tạo không được để trống")]
        [MaxLength(100, ErrorMessage = "Hình thức đào tạo không được vượt quá 100 ký tự")]
        public string TrainingType { get; set; } = string.Empty;

        public bool? IsAnnualSystem { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Thời gian đào tạo không hợp lệ")]
        public int? TrainingDuration { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Số kỳ học không hợp lệ")]
        public int? SemesterPerYear { get; set; }
        public bool? IsCredit { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Số học phần bắt buộc không hợp lệ")]
        public int? MandatoryCourse { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Số học phần tự chọn không hợp lệ")]
        public int? ElectiveCourse { get; set; }
        public bool? Status { get; set; }

        [MaxLength(255, ErrorMessage = "Mô tả không được vượt quá 255 ký tự")]
        public string? Description { get; set; }


    }
}
