namespace ISC_ELIB_SERVER.DTOs.Responses
{
    public class AnswerImagesQaResponse
    {
        public int Id { get; set; }
        public int AnswerId { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
    }
}
