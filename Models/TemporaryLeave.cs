using System;
using System.Collections.Generic;

namespace ISC_ELIB_SERVER.Models
{
    public partial class TemporaryLeave
    {
        public int Id { get; set; }
        public DateTime? Date { get; set; }
        public string? Note { get; set; }
        public string? Attachment { get; set; }
        public bool? Status { get; set; }
        public int? TeacherId { get; set; }
        public int? LeadershipId { get; set; }
        public bool Active { get; set; }

        public virtual TeacherInfo? Teacher { get; set; }
    }
}
