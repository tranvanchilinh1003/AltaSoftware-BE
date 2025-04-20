using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace ISC_ELIB_SERVER.DTOs.Responses
{
    public class ClassTypeResponse
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public bool? Status { get; set; }
        public bool? IsDelete { get; set; }
        public string? Description { get; set; }

        public AcademicYearResponse? AcademicYear { get; set; }

       
    }
}
