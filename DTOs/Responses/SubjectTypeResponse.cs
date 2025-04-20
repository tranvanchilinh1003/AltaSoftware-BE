namespace ISC_ELIB_SERVER.DTOs.Responses
{
    public class SubjectTypeResponse
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public bool? Status { get; set; }
        public string? Description { get; set; }
        public AcademicYearResponse? AcademicYear { get; set; }
    }
}
