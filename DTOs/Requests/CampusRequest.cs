using System.ComponentModel.DataAnnotations;

namespace ISC_ELIB_SERVER.DTOs.Requests
{
    public class CampusRequest
    {
        [Required(ErrorMessage = "Tên cơ sở không được để trống.")]
        [MaxLength(255, ErrorMessage = "Tên cơ sở không được vượt quá 255 ký tự.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Địa chỉ không được để trống.")]
        [MaxLength(500, ErrorMessage = "Địa chỉ không được vượt quá 500 ký tự.")]
        public string? Address { get; set; }

        [Required(ErrorMessage = "Số điện thoại không được để trống.")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ.")]
        [MaxLength(15, ErrorMessage = "Số điện thoại không được vượt quá 15 ký tự.")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Mã trường không được để trống.")]
        [Range(1, int.MaxValue, ErrorMessage = "Mã trường phải là số dương.")]
        public int? SchoolId { get; set; }

        [Required(ErrorMessage = "Mã người dùng không được để trống.")]
        [Range(1, int.MaxValue, ErrorMessage = "Mã người dùng phải là số dương.")]
        public int? UserId { get; set; }
    }
}
