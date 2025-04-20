using System;
using System.ComponentModel.DataAnnotations;
using ISC_ELIB_SERVER.DTOs.Responses;

namespace ISC_ELIB_SERVER.DTOs.Requests
{
    public class TestsSubmissionRequest
    {
        //[Required(ErrorMessage = "TestId không được để trống.")]
        public int? TestId { get; set; }

        //[Required(ErrorMessage = "StudentId không được để trống.")]
        public int? StudentId { get; set; }

        //[Required(ErrorMessage = "Ngày nộp bài không được để trống.")]
        public DateTime? SubmittedAt { get; set; }

        //[Required(ErrorMessage = "Tổng số câu hỏi không được để trống.")]
        //[Range(1, int.MaxValue, ErrorMessage = "Tổng số câu hỏi phải lớn hơn 0.")]
        public int? TotalQuestion { get; set; }

        //[Required(ErrorMessage = "Số câu đúng không được để trống.")]
        //[Range(0, int.MaxValue, ErrorMessage = "Số câu đúng không được âm.")]
        public int? CorrectAnswers { get; set; }

        //[Required(ErrorMessage = "Số câu sai không được để trống.")]
        //[Range(0, int.MaxValue, ErrorMessage = "Số câu sai không được âm.")]
        public int? WrongAnswers { get; set; }

        //[Required(ErrorMessage = "Điểm không được để trống.")]
        //[Range(0, 10, ErrorMessage = "Điểm phải từ 0 đến 10.")]
        public double? Score { get; set; }

        public bool? Graded { get; set; } = false;
        public bool? Active { get; set; } = true;
        public int? UserId { get; set; }

        //public List<TestAnswerResponse> Answers { get; set; } = new List<TestAnswerResponse>();
    }
}
