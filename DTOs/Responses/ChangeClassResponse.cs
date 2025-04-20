namespace ISC_ELIB_SERVER.DTOs.Responses
{
    public class ChangeClassResponse
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int OldClassId { get; set; }
        public DateTime? ChangeClassDate { get; set; }
        public int NewClassId { get; set; }
        public string? Reason { get; set; }
        public string? AttachmentName { get; set; }
        public string? AttachmentPath { get; set; }
        public int LeadershipId { get; set; }

    }
}
