using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Mappers
{
    public class TeacherFamilyMapper : Profile
    {
        public TeacherFamilyMapper()
        {
            // Mapping từ Request -> Model
            CreateMap<TeacherFamilyRequest, TeacherFamily>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()); // Nếu Id tự động tăng, bỏ qua nó.

            // Mapping từ Model -> Response
            CreateMap<TeacherFamily, TeacherFamilyResponse>();
        }
    }
}
