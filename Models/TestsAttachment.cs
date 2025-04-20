using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ISC_ELIB_SERVER.Models
{
    public partial class TestsAttachment
    {
        public int Id { get; set; }
        public int? SubmissionId { get; set; }
        public string? FileUrl { get; set; }
        public bool Active { get; set; }

        public virtual TestsSubmission? Submission { get; set; }
    }
}
