namespace ISC_ELIB_SERVER.DTOs.Responses
{
    public class StudentInfoClassResponse
    {
        public int Id { get; set; }  // ID từ bảng StudentInfo
        public string? Code { get; set; } // Mã học viên từ bảng user
        public string? FullName { get; set; }  // Tên đầy đủ từ bảng User
        public string AcademicYear { get; set; } // Niên khóa (từ năm - đến năm)
        public DateTime? EnrollmentDate { get; set; } // Ngày nhập học từ bảng User
        public string? UserStatusName { get; set; } // Tên trạng thái từ bảng UserStatus

    }
}
