namespace ISC_ELIB_SERVER.DTOs.Responses
{
    public class TopicResponse
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime? EndDate { get; set; }
        public string? File { get; set; }
        public bool Active { get; set; }
    }
}
