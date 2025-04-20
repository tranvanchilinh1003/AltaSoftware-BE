namespace ISC_ELIB_SERVER.DTOs.Responses
{
    public class DiscussionResponse
    {
        public int Id { get; set; }
        public string? Content { get; set; }
        public DateTime? CreateAt { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string TopicTitle { get; set; } = string.Empty;
    }

}
