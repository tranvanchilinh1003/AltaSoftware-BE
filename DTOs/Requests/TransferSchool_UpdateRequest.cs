using System.ComponentModel.DataAnnotations;

namespace ISC_ELIB_SERVER.DTOs.Requests
{
    public class TransferSchool_UpdateRequest
    {
        [Required(ErrorMessage = "Mã sinh viên không được để trống.")]
        [Range(1, int.MaxValue, ErrorMessage = "Mã sinh viên phải lớn hơn 0.")]
        public int StudentId { get; set; }

        [Required(ErrorMessage = "Ngày chuyển trường không được để trống.")]
        [DataType(DataType.DateTime, ErrorMessage = "Ngày chuyển trường không hợp lệ.")]
        public DateTime? TransferSchoolDate { get; set; }

        [Required(ErrorMessage = "Tên trường chuyển đến không được để trống.")]
        [StringLength(255, ErrorMessage = "Tên trường chuyển đến không được vượt quá 255 ký tự.")]
        public string? TransferToSchool { get; set; }

        [StringLength(500, ErrorMessage = "Địa chỉ trường không được vượt quá 500 ký tự.")]
        public string? SchoolAddress { get; set; }

        [StringLength(1000, ErrorMessage = "Lý do chuyển trường không được vượt quá 1000 ký tự.")]
        public string? Reason { get; set; }

        [StringLength(255, ErrorMessage = "Tên file đính kèm không được vượt quá 255 ký tự.")]
        public string? AttachmentName { get; set; }

        [StringLength(500, ErrorMessage = "Đường dẫn file đính kèm không được vượt quá 500 ký tự.")]
        public string? AttachmentPath { get; set; }

        [Required(ErrorMessage = "Mã lãnh đạo không được để trống.")]
        [Range(1, int.MaxValue, ErrorMessage = "Mã lãnh đạo phải lớn hơn 0.")]

        public int? SemesterId { get; set; }
        public int LeadershipId { get; set; }

    }
}
