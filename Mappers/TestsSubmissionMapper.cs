using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Mappers
{
    public class TestsSubmissionMapper : Profile
    {
        public TestsSubmissionMapper() {
            CreateMap<TestsSubmission, TestsSubmissionResponse>()
                .ForMember(dest => dest.TestName, opt => opt.MapFrom(src => src.Test.Name));
            CreateMap<TestsSubmissionRequest, TestsSubmission>();
        }

    }
}
