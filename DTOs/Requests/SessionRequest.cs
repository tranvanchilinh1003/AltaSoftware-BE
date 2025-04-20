using System;
using System.ComponentModel.DataAnnotations;

namespace ISC_ELIB_SERVER.DTOs.Requests
{
    public class SessionRequest
    {
        [Required(ErrorMessage = "session chủ đề không được để trống")]
        [MaxLength(200, ErrorMessage = "Tên không được vượt quá 200 ký tự")]
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? DurationTime { get; set; }
        public string? Password { get; set; }
        public bool? AutoOpen { get; set; }
        public string? ShareCodeUrl { get; set; }
        public string? Status { get; set; }
        public bool? IsExam { get; set; }
        public int? TeachingAssignmentId { get; set; }
        public int? ExamId { get; set; }
        public bool Active { get; set; }
    }

    public class SessionRequestTeacher
    {
        [Required(ErrorMessage = "session chủ đề không được để trống")]
        [MaxLength(200, ErrorMessage = "Tên không được vượt quá 200 ký tự")]
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? DurationTime { get; set; }
        public string? Password { get; set; }
        public bool? AutoOpen { get; set; }
        public string? ShareCodeUrl { get; set; }
        public string? Status { get; set; }
        public bool? IsExam { get; set; }
        public int? ClassId { get; set; }
        public int? teachingAssistantId { get; set; }
        public int? ExamId { get; set; }
        public bool Active { get; set; }
    }
}