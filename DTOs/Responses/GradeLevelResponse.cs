using System.Text.Json.Serialization;

namespace ISC_ELIB_SERVER.DTOs.Responses
{
    public class GradeLevelResponse
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? TeacherId { get; set; }
    }
}
