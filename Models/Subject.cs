using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace ISC_ELIB_SERVER.Models
{
    public partial class Subject
    {
        public Subject()
        {
            ExamSchedules = new HashSet<ExamSchedule>();
            Exams = new HashSet<Exam>();
            QuestionQas = new HashSet<QuestionQa>();
            StudentScores = new HashSet<StudentScore>();
            TeachingAssignments = new HashSet<TeachingAssignment>();
            Tests = new HashSet<Test>();
            ClassSubjects = new HashSet<ClassSubject>();

        }

        public int Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }

        [Column("hours_semester_1")] 
        public int? HoursSemester1 { get; set; }

        [Column("hours_semester_2")]
        public int? HoursSemester2 { get; set; }
        public int? SubjectGroupId { get; set; }
        public int? SubjectTypeId { get; set; }
        public bool Active { get; set; }
        [JsonIgnore]
        public virtual SubjectGroup? SubjectGroup { get; set; }
        [JsonIgnore]
        public virtual SubjectType? SubjectType { get; set; }
        public virtual ICollection<ExamSchedule> ExamSchedules { get; set; }
        public virtual ICollection<Exam> Exams { get; set; }
        public virtual ICollection<QuestionQa> QuestionQas { get; set; }
        public virtual ICollection<StudentScore> StudentScores { get; set; }
        public virtual ICollection<TeachingAssignment> TeachingAssignments { get; set; }
        public virtual ICollection<Test> Tests { get; set; }
        [JsonIgnore]
        public virtual ICollection<ClassSubject> ClassSubjects { get; set; }

    }
}
