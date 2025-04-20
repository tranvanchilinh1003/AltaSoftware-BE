using System.Text.Json.Serialization;

namespace ISC_ELIB_SERVER.DTOs.Responses
{
    public class TrainingProgramsResponse
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        //public string? Description { get; set; }
        public int MajorId { get; set; }
        public int schoolFacilitiesID { get; set; }

        // [JsonConverter(typeof(DateOnlyJsonConverter))]
        public DateTime StartDate { get; set; }
        // [JsonConverter(typeof(DateOnlyJsonConverter))]
        public DateTime EndDate { get; set; }
        public string? Degree { get; set; }
        public string? TrainingForm { get; set; }
        //public bool Active { get; set; } 
        public string? FilePath { get; set; }
        public string? FileName { get; set; }
    }
}
