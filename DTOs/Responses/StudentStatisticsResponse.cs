namespace ISC_ELIB_SERVER.DTOs.Responses
{
    public class StudentStatisticsResponse
    {
        public int TotalClasses { get; set; }
        public int ExcellentStudents { get; set; }
        public int GoodStudents { get; set; }
        public int AverageStudents { get; set; }
        public int WeakStudents { get; set; }
    }
}
