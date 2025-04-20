using System;

namespace ISC_ELIB_SERVER.DTOs.Responses
{
    public class TopicsFileResponse
    {
        public int Id { get; set; }
        public int TopicId { get; set; }
        public string FileUrl { get; set; }
        public string? FileName { get; set; }
        public DateTime? CreateAt { get; set; }
        public bool Active { get; set; }
    }
}
