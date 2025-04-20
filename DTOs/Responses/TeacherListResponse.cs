using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.DTOs.Responses
{
    public class TeacherListResponse
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string TeacherCode { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public DateTime BirthDate { get; set; }
        public bool? Gender { get; set; }
        public int SubjectId { get; set; }
        public string Position { get; set; }

        // Status dạng enum
        public int Status { get; set; }
    }
}
