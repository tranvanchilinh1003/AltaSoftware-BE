using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.DTOs.Responses
{
    public class SubjectGroupResponse
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        public int TeacherId { get; set; }
        public UserResponse? Teacher { get; set; }
    }
}
