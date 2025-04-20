using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;
namespace ISC_ELIB_SERVER.Mappers
{
    public class TeacherInfoMapper : Profile
    {
        public TeacherInfoMapper() {
            CreateMap<TeacherInfoRequest, TeacherInfo>();
            CreateMap<TeacherInfo, TeacherInfoResponses>();
        }
    }
}
