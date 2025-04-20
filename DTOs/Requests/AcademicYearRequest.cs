using System.ComponentModel.DataAnnotations;

namespace ISC_ELIB_SERVER.DTOs.Requests
{
    public class AcademicYearRequest
    {
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Ngày bắt đầu không được để trống")]
        public DateTime StartTime { get; set; }
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Ngày kết thúc không được để trống")]
        public DateTime EndTime { get; set; }
        public ICollection<AcademicYearSemesterCreateRequest>? Semesters { get; set; }
    }

    public class AcademicYearSemesterCreateRequest
    {
        public string? Name { get; set; }
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Ngày bắt đầu không được để trống")]
        public DateTime StartTime { get; set; }
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Ngày kết thúc không được để trống")]
        public DateTime EndTime { get; set; }
    }

    public class AcademicYearSemesterRequest
    {

        public int? Id { get; set; }
        public string? Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

    }

}
