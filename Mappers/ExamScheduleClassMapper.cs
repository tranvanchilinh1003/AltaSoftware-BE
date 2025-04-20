using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Mappers
{
    public class ExamScheduleClassMapper : Profile
    {
        public ExamScheduleClassMapper()
        {
            CreateMap<ExamScheduleClass, ExamScheduleClassResponse>()
                .ForMember(dest => dest.ClassName,
                    opt => opt.MapFrom(src => src.Class != null ? src.Class.Name : string.Empty))
                .ForMember(dest => dest.ExamScheduleName,
                    opt => opt.MapFrom(src => src.ExampleScheduleNavigation != null ? src.ExampleScheduleNavigation.Name : string.Empty))
                .ForMember(dest => dest.SupervisoryTeacherName,
                    opt => opt.MapFrom(src =>
                        src.SupervisoryTeacher != null && src.SupervisoryTeacher.User != null
                        ? src.SupervisoryTeacher.User.FullName
                        : string.Empty))
                // Mapping cho StudentQuantity (giả sử thuộc tính StudentQuantity tồn tại trong Class)
                .ForMember(dest => dest.StudentQuantity,
                    opt => opt.MapFrom(src => src.Class != null ? src.Class.StudentQuantity : 0))
                // Mapping cho ClassCode (giả sử lớp Class có thuộc tính Code)
                .ForMember(dest => dest.ClassCode,
                    opt => opt.MapFrom(src => src.Class != null ? src.Class.Code : string.Empty))
                // Map nested object ExamScheduleDetail
                .ForMember(dest => dest.ExamScheduleDetail,
                    opt => opt.MapFrom(src => src.ExampleScheduleNavigation))
                // Map danh sách tên giáo viên chấm thi từ Exam → ExamGraders → User.FullName
                .ForMember(dest => dest.GradingTeacherNames,
                    opt => opt.MapFrom(src =>
                        src.ExampleScheduleNavigation != null &&
                        src.ExampleScheduleNavigation.Exam != null &&
                        src.ExampleScheduleNavigation.Exam.ExamGraders != null
                            ? src.ExampleScheduleNavigation.Exam.ExamGraders
                                  .Select(eg => eg.User != null ? eg.User.FullName : string.Empty)
                                  .Where(name => !string.IsNullOrEmpty(name))
                                  .ToList()
                            : new List<string>()))
                // Mapping cho JoinedStudentQuantity mới
                .ForMember(dest => dest.joined_student_quantity,
                    opt => opt.MapFrom(src => src.joined_student_quantity));

            CreateMap<ExamScheduleClassRequest, ExamScheduleClass>();

            // Mapping từ ExamSchedule sang ExamScheduleDetailResponse
            CreateMap<ExamSchedule, ExamScheduleDetailResponse>()
                .ForMember(dest => dest.SemesterName,
                    opt => opt.MapFrom(src => src.Semester != null ? src.Semester.Name : string.Empty))
                .ForMember(dest => dest.GradeLevelName,
                    opt => opt.MapFrom(src => src.GradeLevels != null ? src.GradeLevels.Name : string.Empty))
                .ForMember(dest => dest.DurationInMinutes,
                    opt => opt.MapFrom(src => src.duration_in_minutes));
        }
    }

}
