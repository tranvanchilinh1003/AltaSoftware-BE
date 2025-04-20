using System;
using System.Collections.Generic;

namespace ISC_ELIB_SERVER.Models
{
    public partial class WorkProcess
    {
        public int Id { get; set; }
        public int? TeacherId { get; set; }
        public string? Organization { get; set; }
        public int? SubjectGroupsId { get; set; }
        public string? Position { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int[]? Program { get; set; }
        public bool? IsCurrent { get; set; }
        public bool Active { get; set; }

        public virtual TeacherInfo? Teacher { get; set; }
        public SubjectGroup? SubjectGroup { get; set; }
    }
}
