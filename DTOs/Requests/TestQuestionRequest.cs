using System.ComponentModel.DataAnnotations;
using ISC_ELIB_SERVER.Enums;

namespace ISC_ELIB_SERVER.DTOs.Requests
{
    public class TestQuestionRequest
    {
        //[Required(ErrorMessage = "TestId không được để trống.")]
        public int? TestId { get; set; }

        //[Required(ErrorMessage = "Câu hỏi không được để trống.")]
        //[StringLength(1000, ErrorMessage = "Câu hỏi không được quá 1000 ký tự.")]
        public string? QuestionText { get; set; }

        //[Required(ErrorMessage = "Loại câu hỏi không được để trống.")]
        //[RegularExpression("MultipleChoice|Essay", ErrorMessage = "Loại câu hỏi chỉ có thể là 'MultipleChoice' hoặc 'Essay'.")]
        public QuestionType QuestionType { get; set; }
    }
}
