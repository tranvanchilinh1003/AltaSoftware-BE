using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.DTOs.Responses
{
    public class SubjectResponse
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int HoursSemester1 { get; set; }
        public int HoursSemester2 { get; set; }

        public SubjectGroupResponse? SubjectGroup { get; set; }
        public SubjectTypeResponse? SubjectType { get; set; }
    }
}
