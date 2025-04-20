using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ISC_ELIB_SERVER.DTOs.Requests
{
    public class TrainingProgramsRequest
    {
        [Required(ErrorMessage = "Tên chương trình không được để trống")]
        [MaxLength(255, ErrorMessage = "Tên chương trình không được vượt quá 255 ký tự")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Chuyên ngành không được để trống")]
        public int MajorId { get; set; }

        [Required(ErrorMessage = "Cơ sở đào tạo không được để trống")]
        public int SchoolFacilitiesId { get; set; }

        [Required(ErrorMessage = "Ngày bắt đầu không được để trống")]
        // [JsonConverter(typeof(DateOnlyJsonConverter))]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Ngày kết thúc không được để trống")]
        // [JsonConverter(typeof(DateOnlyJsonConverter))]
        public DateTime EndDate { get; set; }
        [Required(ErrorMessage = "Bằng cấp không được để trống")]

        [MaxLength(50, ErrorMessage = "Bằng cấp không được vượt quá 50 ký tự")]
        public string? Degree { get; set; }
        [Required(ErrorMessage = "Hình thức đào tạo không được để trống")]

        [MaxLength(50, ErrorMessage = "Hình thức đào tạo không được vượt quá 50 ký tự")]
        public string? TrainingForm { get; set; }
        [Required(ErrorMessage = "Tên tệp không được để trống")]

        //public bool Active { get; set; }

        [MaxLength(250, ErrorMessage = "Tên tệp không được vượt quá 250 ký tự")]
        public string? FileName { get; set; }
        [Required(ErrorMessage = "Đường dẫn tệp không được để trống")]
        [MaxLength(250, ErrorMessage = "Đường dẫn tệp không được vượt quá 250 ký tự")]
        public string? FilePath { get; set; }

        // public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        // {
        //     if (StartDate.HasValue && EndDate.HasValue && StartDate.Value >= EndDate.Value)
        //     {
        //         yield return new ValidationResult("Ngày bắt đầu phải nhỏ hơn ngày kết thúc.", new[] { nameof(StartDate) });
        //     }
        // }
        public int TeacherId { get; set; }


    }
}
