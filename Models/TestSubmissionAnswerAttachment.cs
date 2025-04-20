using System;
using System.Text.Json.Serialization;

namespace ISC_ELIB_SERVER.Models
{
    public class TestSubmissionAnswerAttachment
    {
        public long Id { get; set; }
        public int TestSubmissionAnswerId { get; set; }
        public string Filename { get; set; } = null!;
        public string FileBase64 { get; set; } = null!;
        public DateTime CreatedAt { get; set; }

        // Navigation back to the parent answer
        [JsonIgnore] // Prevent infinite loop during serialization
        public virtual TestSubmissionsAnswer TestSubmissionAnswer { get; set; } = null!;
    }
}
