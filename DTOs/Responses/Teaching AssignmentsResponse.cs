using ISC_ELIB_SERVER.DTOs.Responses;

public class TeachingAssignmentsResponse
{
    public int Id { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Description { get; set; }
    public bool Active { get; set; }
    public TeachingAssignmentsUserResponse? User { get; set; }
    public TeachingAssignmentsClassResponse? Class { get; set; }
    public TeachingAssignmentsSubjectResponse? Subject { get; set; }
    public SubjectGroupResponse? SubjectGroup { get; set; }
    public TeachingAssignmentsTopicResponse? Topics { get; set; }

    public List<TeachingAssignmentsSessionsResponse>? Sessions { get; set; }

    public TeachingAssignmentsSemesterResponse? Semester { get; set; }
    public class TeachingAssignmentsUserResponse
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string FullName { get; set; }
    }

    public class TeachingAssignmentsClassResponse
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
    }

    public class TeachingAssignmentsSubjectResponse
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }

    public class TeachingAssignmentsTopicResponse
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }

    public class TeachingAssignmentsSessionsResponse
    {
        public int Id { get; set; }
        public string? Name { get; set; }

    }

    public class TeachingAssignmentsSemesterResponse
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public AcademicYearResponse? AcademicYear { get; set; }



    }
}
