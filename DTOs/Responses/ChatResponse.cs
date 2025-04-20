namespace ISC_ELIB_SERVER.DTOs.Responses
{
    public class ChatResponse
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public int UserId { get; set; }
        public int SessionId { get; set; }
        public DateTime? SentAt { get; set; }
    }
}
