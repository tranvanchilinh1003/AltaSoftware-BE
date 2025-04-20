namespace ISC_ELIB_SERVER.DTOs.Requests
{
    public class GradersForClassDto
    {
        public int ClassId { get; set; }
        public List<int> GraderIds { get; set; } = new List<int>();
    }
}
