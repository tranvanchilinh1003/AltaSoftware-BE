namespace ISC_ELIB_SERVER.DTOs.Responses
{
    public class QuestionImagesQaResponse
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
    }
}
