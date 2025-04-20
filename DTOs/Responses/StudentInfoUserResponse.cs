namespace ISC_ELIB_SERVER.DTOs.Responses
{
    public class StudentInfoUserResponse
    {
        public int UserId { get; set; }  // ID từ bảng user
        public string? Code { get; set; } // Mã học viên từ bảng user
        public string? FullName { get; set; }  // Tên đầy đủ từ bảng User
        public DateTime? Dob { get; set; }  // Ngày sinh từ bảng User
        public string? Gender { get; set; }  // Giới tính từ bảng User
        public string? Nation { get; set; }  // Quốc tịch từ bảng User
        public string? ClassName { get; set; } // Tên lớp từ bảng Class
        public int? status { get; set; } // Tên trạng thái từ bảng UserStatus
        public AcademicYearResponse? AcademicYear { get; set; }
    }
}
