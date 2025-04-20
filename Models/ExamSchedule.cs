using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using ISC_ELIB_SERVER.Enums;

namespace ISC_ELIB_SERVER.Models
{
    public partial class ExamSchedule
    {
        public ExamSchedule()
        {
            ExamScheduleClasses = new HashSet<ExamScheduleClass>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public DateTime? ExamDay { get; set; }

        public int duration_in_minutes { get; set; }
        public string? Type { get; set; }
        public bool? Form { get; set; }
        [Column("Status")]  
        public ExamStatus Status { get; set; }
        public int? AcademicYearId { get; set; }
        public int? Subject { get; set; }
        public int? SemesterId { get; set; }
        public int? GradeLevelsId { get; set; }
        [Column("exam_id")]
        public int? ExamId { get; set; }

    

        public bool Active { get; set; }



        public virtual Exam? Exam { get; set; }
        public virtual AcademicYear? AcademicYear { get; set; }
        public virtual GradeLevel? GradeLevels { get; set; }
        public virtual Semester? Semester { get; set; }
        public virtual Subject? SubjectNavigation { get; set; }
        public virtual ICollection<ExamScheduleClass> ExamScheduleClasses { get; set; }
    }
}
