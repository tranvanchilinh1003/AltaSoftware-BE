using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.DTOs.Responses
{
    public class SchoolResponse
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public int? ProvinceId { get; set; }
        public int? DistrictId { get; set; }
        public int? WardId { get; set; }
        public string? ProvinceName { get; set; }
        public string? DistrictName { get; set; }
        public string? WardName { get; set; }
        public bool? HeadOffice { get; set; }
        public string? SchoolType { get; set; }
        public string? PhoneNumber { get; set; }
        //public string? Fax { get; set; }
        public string? Email { get; set; }
        public DateTime? EstablishedDate { get; set; }
        public string? TrainingModel { get; set; }
        public string? WebsiteUrl { get; set; }
        public UserResponseSchool? User { get; set; }
        public EducationLevelResponseSchool? EducationLevel { get; set; }
        public ICollection<CampusResponse>? Campuses { get; set; }
        public ICollection<AcademicYearResponse>? AcademicYears { get; set; }
    }

    public class UserResponseSchool
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
    }

    public class EducationLevelResponseSchool
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? TrainingType { get; set; }

    }
}
