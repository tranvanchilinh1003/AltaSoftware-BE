using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ISC_ELIB_SERVER.Models
{
    public partial class School
    {
        public School()
        {
            AcademicYears = new HashSet<AcademicYear>();
            Campuses = new HashSet<Campus>();
        }

        public int Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public int? ProvinceId { get; set; }
        public int? DistrictId { get; set; }
        public int? WardId { get; set; }
        public bool? HeadOffice { get; set; }
        public string? SchoolType { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Fax { get; set; }
        public string? Email { get; set; }
        public DateTime? EstablishedDate { get; set; }
        public string? TrainingModel { get; set; }
        public string? WebsiteUrl { get; set; }
        public int? UserId { get; set; }
        public int? EducationLevelId { get; set; }
        public bool Active { get; set; }

        public virtual User? User { get; set; }
        public virtual EducationLevel? EducationLevel { get; set; }
        public virtual ICollection<AcademicYear> AcademicYears { get; set; }
        public virtual ICollection<Campus> Campuses { get; set; }
    }

    public enum TrainingModel
    {
        [EnumMember(Value = "Công lập")]
        cônglập,

        [EnumMember(Value = "Dân lập")]
        dânlập
    }
}
