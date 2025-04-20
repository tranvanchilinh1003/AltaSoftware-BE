using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.DTOs.Responses
{
    public class RetirementResponse
    {
        public int Id { get; set; }
        public int TeacherId { get; set; }
        public DateTime? Date { get; set; }
        public string? Note { get; set; }
        public string? Attachment { get; set; }
        public RetirementStatus Status { get; set; }
        public int LeadershipId { get; set; }
    }
}
