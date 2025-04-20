namespace ISC_ELIB_SERVER.DTOs.Responses
{
    public class CalendarDay
    {
        public int Day { get; set; }
        public List<ExamScheduleResponse> Exams { get; set; }
    }
}
