namespace ISC_ELIB_SERVER.DTOs.Responses
{
    public class UserResponse
    {
        public int Id { get; set; }
        public string? Code { get; set; }
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
        public int? GradeLevelId { get; set; }
        public int? EntryType { get; set; }
        public string? AddressFull { get; set; }
        public int? ProvinceCode { get; set; }
        public int? DistrictCode { get; set; }
        public int? WardCode { get; set; }
        public string? Street { get; set; }
        public bool Active { get; set; }
        public string? AvatarUrl { get; set; }
        // Thêm thông tin địa chỉ chi tiết
        //public string? ProvinceName { get; set; }
        //public string? DistrictName { get; set; }
        //public string? WardName { get; set; }
        public string? RoleName { get; set; }

    }

    public class StudentProcessResponse
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string? FullName { get; set; }
        public AcademicYearProcessResponse? AcademicYear { get; set; }
        public ClassProcessResponse? Class { get; set; }
        public int StudentQty { get; set; }
        public int SubjectQty { get; set; }

    }

    public class AcademicYearProcessResponse
    {
        public int? Id { get; set; }
        public string Name => $"{StartTime.Year} - {EndTime.Year}";
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

    }

    public class ClassProcessResponse
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }

        public GradeLevelProcessResponse? GradeLevel { get; set; }
        public ClassTypeProcessResponse? ClassType { get; set; }
        public UserProcessResponse? User { get; set; }
        public string? Description { get; set; }

    }
    public class GradeLevelProcessResponse
    {
        public int? Id { get; set; }
        public string? Name { get; set; }

    }
    public class ClassTypeProcessResponse
    {
        public int? Id { get; set; }
        public string? Name { get; set; }

    }
    public class UserProcessResponse
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
    }
}
