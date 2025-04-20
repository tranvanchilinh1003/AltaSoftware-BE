namespace ISC_ELIB_SERVER.DTOs.Responses
{
    public class DiscussionImageResponse
    {
        public int Id { get; set; }
        public int DiscussionId { get; set; }
        public string ImageUrl { get; set; } = string.Empty;

    }
}
