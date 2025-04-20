using System;
using System.ComponentModel.DataAnnotations;

namespace ISC_ELIB_SERVER.DTOs.Requests
{
    public class TeachingAssignmentsRequest
    {
        [Required(ErrorMessage = "Ngày bắt đầu không được để trống!")]
        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [MaxLength(500, ErrorMessage = "Mô tả không được vượt quá 500 ký tự")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn giảng viên!")]
        public int? UserId { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn lớp học!")]
        public int? ClassId { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn môn học!")]
        public int? SubjectId { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn chủ đề!")]
        public int? TopicsId { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn học kỳ!")]
        public int? SemesterId { get; set; }

    }
}
