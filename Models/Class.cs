using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ISC_ELIB_SERVER.Models
{
    public partial class Class
    {
        public Class()
        {
            ChangeClassNewClasses = new HashSet<ChangeClass>();
            ChangeClassOldClasses = new HashSet<ChangeClass>();
            ExamScheduleClasses = new HashSet<ExamScheduleClass>();
            Exemptions = new HashSet<Exemption>();
            TeachingAssignments = new HashSet<TeachingAssignment>();
            Users = new HashSet<User>();
            ClassSubjects = new HashSet<ClassSubject>();
        }

        public int Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public int? StudentQuantity { get; set; }
        public int? SubjectQuantity { get; set; }
        public string? Description { get; set; }
        public int? GradeLevelId { get; set; }
        public int? AcademicYearId { get; set; }
        public int? UserId { get; set; }
        public int? ClassTypeId { get; set; }
        public bool Active { get; set; }

        public virtual AcademicYear? AcademicYear { get; set; }
        public virtual ClassType? ClassType { get; set; }
        public virtual GradeLevel? GradeLevel { get; set; }
        public virtual User? User { get; set; }
        public virtual ICollection<ChangeClass> ChangeClassNewClasses { get; set; }
        public virtual ICollection<ChangeClass> ChangeClassOldClasses { get; set; }
        public virtual ICollection<ExamScheduleClass> ExamScheduleClasses { get; set; }
        public virtual ICollection<Exemption> Exemptions { get; set; }
        public virtual ICollection<TeachingAssignment> TeachingAssignments { get; set; }
        public virtual ICollection<User> Users { get; set; }
        [JsonIgnore]
        public virtual ICollection<ClassSubject> ClassSubjects { get; set; }

        public virtual ICollection<ClassUser> ClassUsers { get; set; }
    }
}
