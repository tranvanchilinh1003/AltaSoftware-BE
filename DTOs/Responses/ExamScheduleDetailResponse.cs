using ISC_ELIB_SERVER.DTOs.Responses;

public class ExamScheduleDetailResponse
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public DateTime? ExamDay { get; set; }

    // Dùng duy nhất "DurationInMinutes"
    public int DurationInMinutes { get; set; }

    public string? Type { get; set; }
    public bool? Form { get; set; }
    public int Status { get; set; }
    public string StatusName { get; set; } = string.Empty;

    public int AcademicYearId { get; set; }
    public int Subject { get; set; }
    public int SemesterId { get; set; }
    public int GradeLevelsId { get; set; }

    public string? AcademicYear { get; set; }
    // Dùng duy nhất "SemesterName" và "GradeLevelName"
    public string? SemesterName { get; set; }
    public string? GradeLevelName { get; set; }

    public string? SubjectName { get; set; }
    public List<string>? TeacherNames { get; set; }

    public List<ExamScheduleClassDetailDto> ParticipatingClasses { get; set; } = new();
}