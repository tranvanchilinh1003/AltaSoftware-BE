using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ISC_ELIB_SERVER.DTOs.Requests
{
    public class ReserveRequest
    {
        [Required(ErrorMessage = "Mã sinh viên không được để trống")]
        public int StudentId { get; set; }

        [Required(ErrorMessage = "Ngày đặt không được để trống")]
        public DateTime ReserveDate { get; set; }

        [MaxLength(50, ErrorMessage = "Thời gian giữ chỗ không được vượt quá 50 ký tự")]
        public string RetentionPeriod { get; set; } = string.Empty;

        [MaxLength(200, ErrorMessage = "Lý do không được vượt quá 200 ký tự")]
        public string? Reason { get; set; }

        [MaxLength(255, ErrorMessage = "Tên tệp không được vượt quá 255 ký tự")]
        public string? File { get; set; }

        [Required(ErrorMessage = "Mã lớp không được để trống")]
        public int ClassId { get; set; }

        [Required(ErrorMessage = "Mã học kỳ không được để trống")]
        public int? SemesterId { get; set; }

        [JsonIgnore]  // Ẩn khỏi request & response JSON
        public int UserId { get; set; }
    }
}
