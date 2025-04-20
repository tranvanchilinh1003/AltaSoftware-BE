using System;

namespace ISC_ELIB_SERVER.DTOs.Responses
{
    public class TransferSchoolResponse
    {
        public int? StudentId { get; set; }

        public string? FullName { get; internal set; }
        public string? StudentCode { get; set; }  // Lấy từ bảng User
        public DateTime? TransferSchoolDate { get; set; }
        public string? TransferToSchool { get; set; }

        public string? TransferSemester { get; set; }  // Lấy từ bảng Semester

        public int? SemesterId { get; set; }  // Lấy từ bảng Semester
        public string? Reason { get; set; }
      
        public int? ProvinceCode { get; set; }  // Lấy từ bảng User
        public int? DistrictCode { get; set; }  // Lấy từ bảng User

        public string? AttachmentName { get; set; }
        public string? AttachmentPath { get; set; }
        public int StatusCode { get; set; }  // Chỉ dùng để báo lỗi/trạng thái
    }
}
