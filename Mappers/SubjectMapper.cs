using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Mappers
{
    public class SubjectMapper: Profile
    {
        public SubjectMapper() {
            CreateMap<Subject, SubjectResponse>()
                .AfterMap((src, dest, context) =>
                {
                    dest.SubjectGroup = context.Mapper.Map<SubjectGroupResponse>(src.SubjectGroup);
                    dest.SubjectType = context.Mapper.Map<SubjectTypeResponse>(src.SubjectType);
                });
            CreateMap<SubjectRequest, Subject>();
        }
    }
}
