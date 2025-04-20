namespace ISC_ELIB_SERVER.DTOs.Requests
{
    public class SessionStudentFilterRequest
    {
        // public int studentId { get; set; }
        public DateTime? Date { get; set; } // Lọc theo ngày
        public int? SubjectId { get; set; } // Lọc theo môn học
        public int? AcademicYearId { get; set; } // Lọc theo niên khóa
        public string? TopicName { get; set; } // Lọc theo tên topic
        public string? Status { get; set; } // Trạng thái lớp
        public string SortColumn { get; set; } = "Id";
        public string SortOrder { get; set; } = "asc";
    }


}