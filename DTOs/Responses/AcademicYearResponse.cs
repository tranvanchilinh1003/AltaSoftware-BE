using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using ISC_ELIB_SERVER.Models;
using Newtonsoft.Json;

namespace ISC_ELIB_SERVER.DTOs.Responses
{
    public class AcademicYearResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public ICollection<SemesterAcademicYearResponse>? Semesters { get; set; }
    }

    public class SemesterAcademicYearResponse
    {
        public int Id { get; set; }

        public string? Name { get; set; }
        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

    }
}
