using System;
using System.Collections.Generic;

namespace ISC_ELIB_SERVER.Models
{
    public partial class TeacherInfo
    {
        public TeacherInfo()
        {
            ExamScheduleClasses = new HashSet<ExamScheduleClass>();
            GradeLevels = new HashSet<GradeLevel>();
            Resignations = new HashSet<Resignation>();
            Retirements = new HashSet<Retirement>();
            TeacherFamilies = new HashSet<TeacherFamily>();
            TeacherTrainingPrograms = new HashSet<TeacherTrainingProgram>();
            TemporaryLeaves = new HashSet<TemporaryLeave>();
            WorkProcesses = new HashSet<WorkProcess>();
        }

        public int Id { get; set; }
        public string? Cccd { get; set; }
        public DateTime? IssuedDate { get; set; }
        public string? IssuedPlace { get; set; }
        public bool? UnionMember { get; set; }
        public DateTime? UnionDate { get; set; }
        public string? UnionPlace { get; set; }
        public bool? PartyMember { get; set; }
        public DateTime? PartyDate { get; set; }
        public int? UserId { get; set; }

        public string? AddressFull { get; set; }
        public int? ProvinceCode { get; set; }
        public int? DistrictCode { get; set; }
        public int? WardCode { get; set; }
        public bool Active { get; set; }
        public virtual User? User { get; set; }
        public int SubjectId { get; set; } // Môn học
        public string? Position { get; set; } // Chức vụ

        public virtual Subject? Subject { get; set; }
        public virtual ICollection<ExamScheduleClass> ExamScheduleClasses { get; set; }
        public virtual ICollection<GradeLevel> GradeLevels { get; set; }
        public virtual ICollection<Resignation> Resignations { get; set; }
        public virtual ICollection<Retirement> Retirements { get; set; }
        //public virtual ICollection<SubjectGroup> SubjectGroups { get; set; }
        public virtual ICollection<TeacherFamily> TeacherFamilies { get; set; }
        public virtual ICollection<TeacherTrainingProgram> TeacherTrainingPrograms { get; set; }
        public virtual ICollection<TemporaryLeave> TemporaryLeaves { get; set; }
        public virtual ICollection<WorkProcess> WorkProcesses { get; set; }
    }
}
