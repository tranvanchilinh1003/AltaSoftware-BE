using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Requests.ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Mappers
{
    public class TransferSchoolMapper : Profile
    {
        public TransferSchoolMapper()
        {
            // Map từ Request -> Model để lưu vào database
            CreateMap<TransferSchoolRequest, TransferSchool>();

            // Map từ Model -> Response để trả về FE
            CreateMap<TransferSchool, TransferSchoolResponse>();

            // Mapping từ Entity -> Response DTO
            CreateMap<TransferSchool, TransferSchoolResponse>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src =>
                    src.Student != null && src.Student.User != null ? src.Student.User.FullName : null))
                .ForMember(dest => dest.StudentCode, opt => opt.MapFrom(src =>
                    src.Student != null && src.Student.User != null ? src.Student.User.Code : null))
                .ForMember(dest => dest.SemesterId, opt => opt.MapFrom(src =>
                    src.Semester != null ? src.Semester.Name : null));
        }
    }
}
