using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Mappers
{
    public class TestSubmissionAnswerAttachmentMapper : Profile
    {
        public TestSubmissionAnswerAttachmentMapper()
        {
            CreateMap<TestSubmissionsAnswer, TestSubmissionAnswerResponse>()
                        .ForMember(dest => dest.Attachments,
                                   opt => opt.MapFrom(src => src.Attachments));

            // Mapping từ TestSubmissionAnswerAttachmentRequest sang TestSubmissionAnswerAttachment
            CreateMap<TestSubmissionAnswerAttachmentRequest, TestSubmissionAnswerAttachment>();

            // Mapping từ TestSubmissionAnswerAttachment sang TestSubmissionAnswerAttachmentResponse
            CreateMap<TestSubmissionAnswerAttachment, TestSubmissionAnswerAttachmentResponse>();
        }
    }
}
