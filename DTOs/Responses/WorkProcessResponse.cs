using System.ComponentModel.DataAnnotations;

namespace ISC_ELIB_SERVER.DTOs.Responses
{
    public class WorkProcessResponse
    {
        public int Id { get; set; }
        public int TeacherId { get; set; }
        public string? Organization { get; set; }
        public int SubjectGroupsId { get; set; }
        public string? Position { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int[]? Program { get; set; }
        public bool? IsCurrent { get; set; }
    }
}
