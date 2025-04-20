using ISC_ELIB_SERVER.Models;
using System.ComponentModel.DataAnnotations;

namespace ISC_ELIB_SERVER.DTOs.Requests
{
    public class RetirementRequest
    {
        [Required(ErrorMessage = "TeacherId là bắt buộc.")]
        public int TeacherId { get; set; }

        [Required(ErrorMessage = "Ngày không được để trống.")]
        [DataType(DataType.DateTime, ErrorMessage = "Ngày không hợp lệ.")]
        public DateTime? Date { get; set; }

        [StringLength(500, ErrorMessage = "Ghi chú không được vượt quá 500 ký tự.")]
        public string? Note { get; set; }

        public string? Attachment { get; set; }

        public RetirementStatus Status { get; set; } = RetirementStatus.Working;

        [Required(ErrorMessage = "LeadershipId là bắt buộc.")]
        public int LeadershipId { get; set; }

        public bool Active { get; set; }
    }
}
