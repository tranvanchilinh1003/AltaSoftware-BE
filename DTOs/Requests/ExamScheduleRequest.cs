using ISC_ELIB_SERVER.Enums;

namespace ISC_ELIB_SERVER.DTOs.Requests
{
    public class ExamScheduleRequest
    {
        public string? Name { get; set; }

        public int? ExamId { get; set; }
        public DateTime? ExamDay { get; set; }
        public string? Type { get; set; }
        public bool? Form { get; set; }
        public ExamScheduleStatus? Status { get; set; }
        public int AcademicYearId { get; set; }
        public int Subject { get; set; }
        public List<int> SemesterIds { get; set; } = new List<int>();
        public int GradeLevelsId { get; set; }
        public int duration_in_minutes { get; set; }
        public List<int> ClassIds { get; set; } = new List<int>();
        public List<GradersForClassDto> GradersForClasses { get; set; } = new List<GradersForClassDto>();
    }
}
