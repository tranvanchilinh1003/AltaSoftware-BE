using System.ComponentModel.DataAnnotations;

namespace ISC_ELIB_SERVER.DTOs.Requests
{
    public class WorkProcessRequest
    {

        [Required(ErrorMessage = "Mã giáo viên đang bị bỏ trống.")]
        [Range(1, int.MaxValue, ErrorMessage = "Mã giáo viên phải là số nguyên dương.")]
        public int TeacherId { get; set; }

        [Required(ErrorMessage = "Tên tổ chức không được để trống.")]
        [StringLength(100, ErrorMessage = "Tên tổ chức không được vượt quá 100 ký tự.")]
        public string Organization { get; set; }

        [Required(ErrorMessage = "Mã nhóm môn học không được để trống.")]
        [Range(1, int.MaxValue, ErrorMessage = "Mã nhóm môn học phải là số nguyên dương.")]
        public int SubjectGroupsId { get; set; }

        [Required(ErrorMessage = "Chức vụ không được để trống.")]
        [StringLength(50, ErrorMessage = "Chức vụ không được vượt quá 50 ký tự.")]
        public string Position { get; set; }

        [Required(ErrorMessage = "Ngày bắt đầu không được để trống.")]
        [DataType(DataType.DateTime, ErrorMessage = "Ngày bắt đầu không hợp lệ.")]
        public DateTime StartDate { get; set; }

        [DataType(DataType.DateTime, ErrorMessage = "Ngày kết thúc không hợp lệ.")]
        public DateTime EndDate { get; set; }

        public int[]? Program { get; set; }

        [Required(ErrorMessage = "Trạng thái công tác không được để trống.")]
        public bool IsCurrent { get; set; }

        [Required(ErrorMessage = "Trạng thái xóa không được để trống.")]
        public bool Active { get; set; }

    }
}
