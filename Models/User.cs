using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ISC_ELIB_SERVER.Models
{
    public partial class User
    {
        public User()
        {
            Achievements = new HashSet<Achievement>();
            Campuses = new HashSet<Campus>();
            ChangeClasses = new HashSet<ChangeClass>();
            Classes = new HashSet<Class>();
            Discussions = new HashSet<Discussion>();
            ExamGraders = new HashSet<ExamGrader>();
            Exemptions = new HashSet<Exemption>();
            NotificationSenders = new HashSet<Notification>();
            NotificationUsers = new HashSet<Notification>();
            Reserves = new HashSet<Reserve>();
            Resignations = new HashSet<Resignation>();
            Retirements = new HashSet<Retirement>();
            StudentInfos = new HashSet<StudentInfo>();
            StudentScores = new HashSet<StudentScore>();
            Supports = new HashSet<Support>();
            SystemSettings = new HashSet<SystemSetting>();
            TeacherInfos = new HashSet<TeacherInfo>();
            TeachingAssignments = new HashSet<TeachingAssignment>();
            Tests = new HashSet<Test>();
            TestsSubmissions = new HashSet<TestsSubmission>();
            TransferSchools = new HashSet<TransferSchool>();
        }

        public int Id { get; set; }
        public string? Code { get; set; }
        public string? Password { get; set; }
        public string? FullName { get; set; }
        public DateTime? Dob { get; set; }
        public bool? Gender { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? PlaceBirth { get; set; }
        public string? Nation { get; set; }
        public string? Religion { get; set; }
        public DateTime? EnrollmentDate { get; set; }
        public int? RoleId { get; set; }
        public int? AcademicYearId { get; set; }
        public int? UserStatusId { get; set; }
        public int? ClassId { get; set; }
        public int? EntryType { get; set; }
        public string? AddressFull { get; set; }
        public int? ProvinceCode { get; set; }
        public int? DistrictCode { get; set; }
        public int? WardCode { get; set; }
        public string? Street { get; set; }
        public bool Active { get; set; }
        [Column("avatar_url")]
        public string? AvatarUrl { get; set; }



        public virtual Class? Class { get; set; }
        public virtual AcademicYear? AcademicYear { get; set; }
        public virtual EntryType? EntryTypeNavigation { get; set; }
        public virtual Role? Role { get; set; }
        public virtual UserStatus? UserStatus { get; set; }
        public virtual ICollection<RefreshToken>? RefreshTokens { get; set; }
        public virtual ICollection<Achievement> Achievements { get; set; }
        public virtual ICollection<Campus> Campuses { get; set; }
        public virtual ICollection<ChangeClass> ChangeClasses { get; set; }
        public virtual ICollection<Class> Classes { get; set; }
        public virtual ICollection<Discussion> Discussions { get; set; }
        public virtual ICollection<ExamGrader> ExamGraders { get; set; }
        public virtual ICollection<Exemption> Exemptions { get; set; }
        public virtual ICollection<Notification> NotificationSenders { get; set; }
        public virtual ICollection<Notification> NotificationUsers { get; set; }
        public virtual ICollection<Reserve> Reserves { get; set; }
        public virtual ICollection<Resignation> Resignations { get; set; }
        public virtual ICollection<Retirement> Retirements { get; set; }
        public virtual ICollection<StudentInfo> StudentInfos { get; set; }
        public virtual ICollection<StudentScore> StudentScores { get; set; }
        public virtual ICollection<Support> Supports { get; set; }
        public virtual ICollection<SystemSetting> SystemSettings { get; set; }
        public virtual ICollection<TeacherInfo> TeacherInfos { get; set; }
        public virtual ICollection<TeachingAssignment> TeachingAssignments { get; set; }
        public virtual ICollection<Test> Tests { get; set; }
        public virtual ICollection<TestsSubmission> TestsSubmissions { get; set; }
        public virtual ICollection<TransferSchool> TransferSchools { get; set; }
        // public virtual School? School { get; set; }  // Liên kết tới School || 
        [JsonIgnore]
        public virtual ICollection<TestUser> TestUsers { get; set; }

        public virtual ICollection<ClassUser> ClassUsers { get; set; }
        public virtual ICollection<SubjectGroup> SubjectGroups { get; set; }

        public virtual ICollection<Session> Sessions { get; set; }

    }
}
