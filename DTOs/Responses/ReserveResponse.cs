namespace ISC_ELIB_SERVER.DTOs.Responses
{
    public class ReserveResponse
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public DateTime ReserveDate { get; set; }
        public string RetentionPeriod { get; set; }
        public string? Reason { get; set; }
        public string? File { get; set; }
        public int ClassId { get; set; }
        public int SemesterId { get; set; }
        public int UserId { get; set; }
        public bool Active { get; set; }
    }
}
