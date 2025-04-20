namespace ISC_ELIB_SERVER.DTOs.Responses
{
    public class ResignationResponse
    {
        public int Id { get; set; }
        public DateTime? Date { get; set; }
        public string? Note { get; set; }
        public string? Attachment { get; set; }
        public bool? Status { get; set; }
        public int TeacherId { get; set; }
        public int LeadershipId { get; set; }
    }
}
