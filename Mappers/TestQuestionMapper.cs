using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Mappers
{
    public class TestQuestionMapper : Profile
    {
        public TestQuestionMapper()
        {
            CreateMap<TestQuestion, TestQuestionResponse>()
                .ForMember(dest => dest.QuestionType, opt => opt.MapFrom(src => src.QuestionType.ToString()))
                .ForMember(dest => dest.TestAnswers, opt => opt.MapFrom(src => src.TestAnswers));

            // Ánh xạ từng câu trả lời (TestAnswer) sang TestAnswerResponse
            CreateMap<TestAnswer, TestAnswerResponse>();

            // Ánh xạ yêu cầu tạo câu hỏi
            CreateMap<TestQuestionRequest, TestQuestion>();
        }
    }
}
