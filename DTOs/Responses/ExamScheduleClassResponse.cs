namespace ISC_ELIB_SERVER.DTOs.Responses
{
    public class ExamScheduleClassResponse
    {
        public int Id { get; set; }

        // Các thông tin ID ban đầu
        public int ClassId { get; set; }
        public int ExamScheduleId { get; set; }
        public int SupervisoryTeacherId { get; set; }

        // Thông tin bổ sung để giao diện hiển thị
        public string? ClassName { get; set; }
        public string? ExamScheduleName { get; set; }
        public string? SupervisoryTeacherName { get; set; }

        public int StudentQuantity { get; set; }
        public int joined_student_quantity { get; set; }
        public string? ClassCode { get; set; }

        public ExamScheduleDetailResponse? ExamScheduleDetail { get; set; }

        public List<string>? GradingTeacherNames { get; set; }
    }

}
