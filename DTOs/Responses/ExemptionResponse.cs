namespace ISC_ELIB_SERVER.DTOs.Responses
{
    public class ExemptionResponse
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int ClassId { get; set; }
        public string? ExemptedObjects { get; set; }
        public string? FormExemption { get; set; }

    }
}
