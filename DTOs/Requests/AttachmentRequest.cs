using System.ComponentModel.DataAnnotations;

namespace ISC_ELIB_SERVER.DTOs.Requests
{

    public class TestSubmissionAnswerAttachmentRequest
    {
        [Required]
        public string Filename { get; set; } = null!;

        [Required]
        public string FileBase64 { get; set; } = null!;
    }
}
