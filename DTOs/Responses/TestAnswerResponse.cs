namespace ISC_ELIB_SERVER.DTOs.Responses
{
    public class TestAnswerResponse
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public string AnswerText { get; set; } = "";
        public bool IsCorrect { get; set; }

        public List<TestAnswerResponse> Answers { get; set; } = new List<TestAnswerResponse>();
       
    }
}