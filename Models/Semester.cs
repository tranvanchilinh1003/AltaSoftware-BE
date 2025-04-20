using System;
using System.Collections.Generic;

namespace ISC_ELIB_SERVER.Models
{
    public partial class Semester
    {
        public Semester()
        {
            ExamSchedules = new HashSet<ExamSchedule>();
            Exams = new HashSet<Exam>();
            StudentScores = new HashSet<StudentScore>();
            TeachingAssignments = new HashSet<TeachingAssignment>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int? AcademicYearId { get; set; }
        public bool Active { get; set; }

        public virtual AcademicYear? AcademicYear { get; set; }
        public virtual ICollection<ExamSchedule> ExamSchedules { get; set; }
        public virtual ICollection<Exam> Exams { get; set; }
        public virtual ICollection<StudentScore> StudentScores { get; set; }
        public virtual ICollection<TeachingAssignment> TeachingAssignments { get; set; }
    }
}
