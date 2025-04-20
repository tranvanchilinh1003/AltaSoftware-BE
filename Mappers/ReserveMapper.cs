using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Mappers
{
	public class ReserveMapper : Profile
	{
		public ReserveMapper()
		{
			// Reserve to ReserveResponse
			CreateMap<Reserve, ReserveResponse>();
			// ReserveRequest to Reserve
			CreateMap<ReserveRequest, Reserve>();

            CreateMap<Reserve, ReserveListResponse>()
               .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Student != null ? src.Student.Code : null)) // Lấy mã học viên từ User
               .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Student != null ? src.Student.FullName : null))
               .ForMember(dest => dest.Dob, opt => opt.MapFrom(src => src.Student != null ? src.Student.Dob : null))
               .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Student != null ? (src.Student.Gender == true ? "Nam" : "Nữ") : null))
               .ForMember(dest => dest.ClassName, opt => opt.MapFrom(src => src.Class != null ? src.Class.Name : null)) // Sửa lỗi ở đây
               .ForMember(dest => dest.ReserveDate, opt => opt.MapFrom(src => src.ReserveDate))
               .ForMember(dest => dest.RetentionPeriod, opt => opt.MapFrom(src => src.RetentionPeriod))
               .ForMember(dest => dest.Reason, opt => opt.MapFrom(src => src.Reason));
        }
    }
}
