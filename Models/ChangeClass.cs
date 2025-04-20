using System;
using System.Collections.Generic;

namespace ISC_ELIB_SERVER.Models
{
    public partial class ChangeClass
    {
        public int Id { get; set; }
        public int? StudentId { get; set; }
        public int? OldClassId { get; set; }
        public DateTime? ChangeClassDate { get; set; }
        public int? NewClassId { get; set; }
        public string? Reason { get; set; }
        public string? AttachmentName { get; set; }
        public string? AttachmentPath { get; set; }
        public int? LeadershipId { get; set; }
        public bool Active { get; set; }

        public virtual Class? NewClass { get; set; }
        public virtual Class? OldClass { get; set; }
        public virtual User? Student { get; set; }
    }
}
