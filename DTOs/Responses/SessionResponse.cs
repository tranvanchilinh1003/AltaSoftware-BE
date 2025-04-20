namespace ISC_ELIB_SERVER.DTOs.Responses
{
    public class SessionResponse
    {
        public int Id { get; set; }
        public string? Name { get; set; }
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
}