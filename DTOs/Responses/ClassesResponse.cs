using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;

public class ClassesResponse
{
    public int? Id { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
    public int? StudentQuantity { get; set; }
    public int? SubjectQuantity { get; set; }
    public string? Description { get; set; }
    public bool Active { get; set; }
    public GradeLevelResponse? GradeLevel { get; set; }

    public AcademicYearResponse? AcademicYear { get; set; }
    //danh sách giảng viên
    public ClassUserResponse? User { get; set; }

    public ClassTypeResponse? ClassType { get; set; }

    public ICollection<ClassSubjectResponse>? Subjects { get; set; }

    //danh sách sinh viên
    public ICollection<ClassUserResponse>? Student { get; set; }
}

public class ClassUserResponse
{
    public int Id { get; set; }
    public string? Code { get; set; }
    public string FullName { get; set; }
    public string? Year { get; set; }
    public string? EnrollmentDate { get; set; }
    public string? UserStatus { get; set; }
}
public class ClassSubjectResponse
{
    public int Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public int HoursSemester1 { get; set; }
    public int HoursSemester2 { get; set; }

}


