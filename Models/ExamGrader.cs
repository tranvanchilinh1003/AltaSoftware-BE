using System;
using System.Collections.Generic;

namespace ISC_ELIB_SERVER.Models
{
    public partial class ExamGrader
    {
        public int Id { get; set; }
        public int? ExamId { get; set; }
        public int? UserId { get; set; }
        public string? ClassIds { get; set; }
        public int? ExamScheduleClassId { get; set; }
        public bool Active { get; set; }
        public virtual ExamScheduleClass? ExamScheduleClass { get; set; }
        public virtual Exam? Exam { get; set; }
        public virtual User? User { get; set; }
    }
}
