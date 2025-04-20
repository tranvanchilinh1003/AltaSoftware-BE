using ISC_ELIB_SERVER.Models;
using System.Text.Json.Serialization;

namespace ISC_ELIB_SERVER.DTOs.Responses
{
    public class TestResponse
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? Type { get; set; }
        public int? DurationTime { get; set; }
        public int? Classify { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string? File { get; set; }
        public string? Description { get; set; }
        public string? ClassIds { get; set; }
        public bool? FileSubmit { get; set; }
        public SubjectResponse? Subject { get; set; }
        public UserResponse? Teacher { get; set; }
        public GradeLevelResponse? GradeLevel { get; set; }
    }
}
