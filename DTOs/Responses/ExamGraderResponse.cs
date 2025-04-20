namespace ISC_ELIB_SERVER.DTOs.Responses
{
    public class ExamGraderResponse
    {
        public int Id { get; set; }
        public int ExamId { get; set; }
        public int UserId { get; set; }
        public string? ClassIds { get; set; }

        public string? UserName { get; set; }
    }
}
