using System.ComponentModel.DataAnnotations;

namespace ISC_ELIB_SERVER.DTOs.Requests
{
    public class ChangeClass_UpdateRequest
    {
        [Required(ErrorMessage = "Mã sinh viên không được để trống.")]
        [Range(1, int.MaxValue, ErrorMessage = "Mã sinh viên phải lớn hơn 0.")]
        public int StudentId { get; set; }

        [Required(ErrorMessage = "Mã lớp cũ không được để trống.")]
        [Range(1, int.MaxValue, ErrorMessage = "Mã lớp cũ phải lớn hơn 0.")]
        public int OldClassId { get; set; }

        [Required(ErrorMessage = "Ngày chuyển lớp không được để trống.")]
        [DataType(DataType.DateTime, ErrorMessage = "Ngày chuyển lớp không hợp lệ.")]
        public DateTime ChangeClassDate { get; set; }

        [Required(ErrorMessage = "Mã lớp mới không được để trống.")]
        [Range(1, int.MaxValue, ErrorMessage = "Mã lớp mới phải lớn hơn 0.")]
        public int NewClassId { get; set; }

        [Required(ErrorMessage = "Lý do chuyển lớp không được để trống.")]
        [StringLength(500, ErrorMessage = "Lý do chuyển lớp không được vượt quá 500 ký tự.")]
        public string? Reason { get; set; }

        [StringLength(255, ErrorMessage = "Tên file đính kèm không được vượt quá 255 ký tự.")]
        public string? AttachmentName { get; set; }

        [StringLength(500, ErrorMessage = "Đường dẫn file đính kèm không được vượt quá 500 ký tự.")]
        public string? AttachmentPath { get; set; }

        [Required(ErrorMessage = "Mã lãnh đạo không được để trống.")]
        [Range(1, int.MaxValue, ErrorMessage = "Mã lãnh đạo phải lớn hơn 0.")]
        public int LeadershipId { get; set; }

    }
}
