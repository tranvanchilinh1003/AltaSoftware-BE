using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Mappers
{
    public class TestSubmissionAnswerMapper : Profile
    {
        public TestSubmissionAnswerMapper()
        {
            CreateMap<TestsSubmission, TestsSubmissionResponse>()
                .ForMember(dest => dest.TestSubmissionAnswer,
                    opt => opt.MapFrom(src => src.TestSubmissionsAnswers));

            CreateMap<TestSubmissionAnswerRequest, TestSubmissionsAnswer>()
                .ForMember(dest => dest.Attachments, opt => opt.Ignore());

            CreateMap<TestSubmissionsAnswer, TestSubmissionAnswerResponse>();

            CreateMap<TestSubmissionAnswerAttachment, TestSubmissionAnswerAttachmentResponse>();
        }
    }
}
