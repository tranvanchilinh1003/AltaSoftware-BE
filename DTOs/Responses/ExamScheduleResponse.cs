using ISC_ELIB_SERVER.Enums;

namespace ISC_ELIB_SERVER.DTOs.Responses
{
    public class ExamScheduleResponse
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public DateTime? ExamDay { get; set; }
        public int duration_in_minutes { get; set; }
        public string? Type { get; set; }
        public bool? Form { get; set; }
        public ExamScheduleStatus Status { get; set; }  // Thay đổi ở đây
        public string StatusName => Status.ToString();
        public int AcademicYearId { get; set; }
        public int Subject { get; set; }
        public List<int> SemesterIds { get; set; } = new List<int>();
        public int GradeLevelsId { get; set; }

        public string? AcademicYear { get; set; }
        public string? SubjectName { get; set; }
        public List<string> SemesterNames { get; set; } = new List<string>();
        public string? GradeLevel { get; set; }
        public List<string>? ClassNames { get; set; }
        public List<string>? TeacherNames { get; set; }

        public List<int>? ClassIds { get; set; }
        public List<GradersForClassResponse>? GradersForClasses { get; set; }
    }

    public class GradersForClassResponse
    {
        public int ClassId { get; set; }
        public List<int> GraderIds { get; set; } = new List<int>();
    }

}
