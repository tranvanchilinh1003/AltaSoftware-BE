using System.ComponentModel.DataAnnotations;

namespace ISC_ELIB_SERVER.DTOs.Requests
{
    public class TestsAttachmentRequest
    {
        [Required(ErrorMessage = "Mã số nộp bài không được bỏ trống")]
        public int SubmissionId { get; set; }

        [Required(ErrorMessage = "Đường dẫn không được bỏ trống")]
        public string FileUrl { get; set; } = null!;

        public bool Active { get; set; }
    }
}
