using System;
using System.ComponentModel.DataAnnotations;

namespace ISC_ELIB_SERVER.DTOs.Requests
{
    public class SchoolRequest
    {
        [Required(ErrorMessage = "Mã trường không được để trống")]
        [MaxLength(50, ErrorMessage = "Mã trường tối đa 50 ký tự")]
        public string? Code { get; set; }

        [Required(ErrorMessage = "Tên trường không được để trống")]
        [MaxLength(255, ErrorMessage = "Tên trường tối đa 255 ký tự")]
        public string? Name { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Tỉnh/Thành phố không hợp lệ")]
        public int ProvinceId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Quận/Huyện không hợp lệ")]
        public int DistrictId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Phường/Xã không hợp lệ")]
        public int WardId { get; set; }

        public bool? HeadOffice { get; set; }

        [MaxLength(100, ErrorMessage = "Loại trường tối đa 100 ký tự")]
        public string? SchoolType { get; set; }

        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        public string? PhoneNumber { get; set; }

        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string? Email { get; set; }

        public DateTime EstablishedDate { get; set; }

        [MaxLength(255, ErrorMessage = "Mô hình đào tạo tối đa 255 ký tự")]
        public string? TrainingModel { get; set; }

        [Url(ErrorMessage = "URL website không hợp lệ")]
        public string? WebsiteUrl { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Người dùng không hợp lệ")]
        public int? UserId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Cấp giáo dục không hợp lệ")]
        public int? EducationLevelId { get; set; }
    }
}
