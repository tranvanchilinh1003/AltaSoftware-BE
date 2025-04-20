namespace ISC_ELIB_SERVER.DTOs.Responses
{
    public class ExamScheduleClassDetailDto
    {
        public int ClassId { get; set; }
        public string? ClassCode { get; set; } // <-- mã lớp
        public string? ClassName { get; set; }
        public string? SupervisoryTeacherName { get; set; } // <-- giáo viên chủ nhiệm
        public int StudentQuantity { get; set; }
        public int JoinedExamStudentQuantity { get; set; }
        public List<string> ExamGraders { get; set; } = new(); // <-- giáo viên chấm thi theo lớp
    }
}
