using System.ComponentModel.DataAnnotations;

namespace ISC_ELIB_SERVER.DTOs.Requests
{
    public class UserUpdateRequest
    {

        [Required(ErrorMessage = "Tên đầy đủ không được để trống")]
        public string? FullName { get; set; }

        public DateTime? Dob { get; set; }

        public bool? Gender { get; set; }

        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }

        public string? PlaceBirth { get; set; }

        public string? Nation { get; set; }

        public string? Religion { get; set; }

        public DateTime? EnrollmentDate { get; set; }

        public int RoleId { get; set; }

        public int? AcademicYearId { get; set; }

        public int? UserStatusId { get; set; }

        public int? ClassId { get; set; }

        public int EntryType { get; set; }

        public string? AddressFull { get; set; }

        public int ProvinceCode { get; set; }

        public int DistrictCode { get; set; }

        public int WardCode { get; set; }

        public string? Street { get; set; }

        public bool Active { get; set; }

        public string? AvatarUrl { get; set; }
    }
}
