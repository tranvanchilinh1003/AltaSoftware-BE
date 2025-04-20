namespace ISC_ELIB_SERVER.DTOs.Responses
{
    public class TestSubmissionAnswerAttachmentResponse
    {
        public long Id { get; set; }

        public string Filename { get; set; } = null!;

        public string FileBase64 { get; set; } = null!;

        public DateTime CreatedAt { get; set; }
    }
}
