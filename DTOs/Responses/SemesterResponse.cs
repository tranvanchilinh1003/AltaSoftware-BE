namespace ISC_ELIB_SERVER.DTOs.Responses
{
    public class SemesterResponse
    {
        public int Id { get; set; }

        public string? Name { get; set; }
        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public int AcademicYearId { get; set; }
    }
}
