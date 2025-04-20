using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.DTOs.Responses
{
    public class TestQuestionResponse
    {


        public int Id { get; set; }
        public int TestId { get; set; }
        public string? QuestionText { get; set; }
        public string? QuestionType { get; set; }

        public ICollection<TestAnswerResponse>? TestAnswers { get; set; }
    }
}
