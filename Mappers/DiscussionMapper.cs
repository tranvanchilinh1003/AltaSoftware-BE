using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Mappers
{
    public class DiscussionMapper : Profile
    {
        public DiscussionMapper()
        {
            // Map từ `Discussion` sang `DiscussionResponse`
            CreateMap<Discussion, DiscussionResponse>();

            // Map từ `DiscussionRequest` sang `Discussion`
            CreateMap<DiscussionRequest, Discussion>();
        }
    }
}
