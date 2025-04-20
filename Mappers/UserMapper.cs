using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Mappers
{
    public class UserMapper : Profile
    {
        public UserMapper()
        {
            // userRequest -> user entity (dùng khi tạo mới hoặc cập nhật người dùng)
            CreateMap<UserRequest, User>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()); // Nếu muốn bỏ qua thuộc tính Id, ví dụ trong tạo mới người dùng

            // user entity -> userResponse (dùng khi trả dữ liệu cho client)
            CreateMap<User, UserResponse>();
            //
            CreateMap<User, StudentProcessResponse>();
            CreateMap<AcademicYear, AcademicYearProcessResponse>();
            CreateMap<Class, ClassProcessResponse>();
            CreateMap<GradeLevel, GradeLevelProcessResponse>();
            CreateMap<ClassType, ClassTypeProcessResponse>();
            CreateMap<User, UserProcessResponse>();
        }
    }
}
