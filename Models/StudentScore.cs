using System;
using System.Collections.Generic;

namespace ISC_ELIB_SERVER.Models
{
    public partial class StudentScore
    {
        public int Id { get; set; }
        public double? Score { get; set; }
        public int? ScoreTypeId { get; set; }
        public int? SubjectId { get; set; }
        public int? UserId { get; set; }
        public int? SemesterId { get; set; }
        public bool Active { get; set; }

        public virtual ScoreType? ScoreType { get; set; }
        public virtual Semester? Semester { get; set; }
        public virtual Subject? Subject { get; set; }
        public virtual User? User { get; set; }
    }
}
