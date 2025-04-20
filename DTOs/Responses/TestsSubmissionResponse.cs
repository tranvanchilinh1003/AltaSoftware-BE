namespace ISC_ELIB_SERVER.DTOs.Responses
{
    public class TestsSubmissionResponse
    {
        public int Id { get; set; }
        public int TestId { get; set; }
        public string? TestName { get; set; }
        public int StudentId { get; set; }
        public string? StudentName { get; set; }
        public DateTime SubmittedAt { get; set; }
        public int TotalQuestion { get; set; }
        public int? CorrectAnswers { get; set; }
        public int? WrongAnswers { get; set; }
        public double? Score { get; set; }
        public bool Graded { get; set; }
        public int? UserId { get; set; }
        public List<TestSubmissionAnswerResponse> TestSubmissionAnswer { get; set; } = new List<TestSubmissionAnswerResponse>();
        //public List<TestAnswerResponse> Answers { get; set; } = new List<TestAnswerResponse>();
        //public List<TestAttachmentResponse> Attachments { get; set; } = new List<TestAttachmentResponse>();
    }
}
