using System.ComponentModel.DataAnnotations;

namespace ISC_ELIB_SERVER.DTOs.Requests
{
    public class TemporaryLeave_AddRequest
    {
        [Required(ErrorMessage = "Ngày nghỉ không được để trống.")]
        [DataType(DataType.DateTime, ErrorMessage = "Ngày nghỉ không hợp lệ.")]
        public DateTime? Date { get; set; }

        [StringLength(1000, ErrorMessage = "Ghi chú không được vượt quá 1000 ký tự.")]
        public string? Note { get; set; }

        [StringLength(255, ErrorMessage = "Tên file đính kèm không được vượt quá 255 ký tự.")]
        public string? Attachment { get; set; }

        [Required(ErrorMessage = "Trạng thái không được để trống.")]
        public bool? Status { get; set; }

        [Required(ErrorMessage = "Mã giáo viên không được để trống.")]
        [Range(1, int.MaxValue, ErrorMessage = "Mã giáo viên phải lớn hơn 0.")]
        public int TeacherId { get; set; }

        [Required(ErrorMessage = "Mã lãnh đạo không được để trống.")]
        [Range(1, int.MaxValue, ErrorMessage = "Mã lãnh đạo phải lớn hơn 0.")]
        public int LeadershipId { get; set; }
    }

}
