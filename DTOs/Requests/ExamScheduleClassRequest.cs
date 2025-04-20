namespace ISC_ELIB_SERVER.DTOs.Requests
{
    public class ExamScheduleClassRequest
    {
        public int ClassId { get; set; }
        public int ExamScheduleId { get; set; }
        public int SupervisoryTeacherId { get; set; }
    }
}
