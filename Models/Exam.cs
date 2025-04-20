using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using ISC_ELIB_SERVER.Enums;

namespace ISC_ELIB_SERVER.Models
{
    public partial class Exam
    {
        public Exam()
        {
            ExamGraders = new HashSet<ExamGrader>();
            Sessions = new HashSet<Session>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public DateTime? ExamDate { get; set; }
        public int? DurationMinutes { get; set; }

        [Column("status")]
        public ExamStatus Status { get; set; }
        public string? File { get; set; }
        public int? SemesterId { get; set; }
        public int? AcademicYearId { get; set; }
        public int? GradeLevelId { get; set; }
        public int? ClassTypeId { get; set; }
        public int? SubjectId { get; set; }
        public bool Active { get; set; }

        public virtual AcademicYear? AcademicYear { get; set; }
        public virtual ClassType? ClassType { get; set; }
        public virtual GradeLevel? GradeLevel { get; set; }
        public virtual Semester? Semester { get; set; }
        public virtual Subject? Subject { get; set; }
        public virtual ICollection<ExamGrader> ExamGraders { get; set; }
        public virtual ICollection<Session> Sessions { get; set; }
    }
}
