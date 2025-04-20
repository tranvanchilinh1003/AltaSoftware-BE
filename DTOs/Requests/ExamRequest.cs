using System.ComponentModel.DataAnnotations;
using ISC_ELIB_SERVER.Enums;

namespace ISC_ELIB_SERVER.DTOs.Requests
{
    public class ExamRequest
    {

        [Required(ErrorMessage = "Tên bài kiểm tra không được để trống")]
        [StringLength(255, ErrorMessage = "Tên bài kiểm tra không được quá 255 ký tự")]
        public string Name { get; set; }

        [Range(1, 300, ErrorMessage = "Thời gian làm bài phải từ 1 đến 300 phút")]
        public int? DurationMinutes { get; set; }

        [Required(ErrorMessage = "Trạng thái không được để trống")]
        public ExamStatus Status { get; set; }

        public IFormFile? File { get; set; }

        [Required(ErrorMessage = "Mã học kỳ không được để trống")]
        public int SemesterId { get; set; }

        [Required(ErrorMessage = "Mã năm học không được để trống")]
        public int AcademicYearId { get; set; }

        [Required(ErrorMessage = "Mã lớp không được để trống")]
        public int GradeLevelId { get; set; }

        [Required(ErrorMessage = "Mã loại lớp không được để trống")]
        public int ClassTypeId { get; set; }

        [Required(ErrorMessage = "Mã môn học không được để trống")]
        public int SubjectId { get; set; }

        public bool Active { get; set; }
    }

}
