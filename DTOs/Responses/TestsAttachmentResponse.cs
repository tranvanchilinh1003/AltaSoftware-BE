namespace ISC_ELIB_SERVER.DTOs.Responses
{
    public class TestsAttachmentResponse
    {
        public int Id { get; set; }
        public string SubmissionId { get; set; } = null!;
        public string FileUrl { get; set; } = null!;
    }
}