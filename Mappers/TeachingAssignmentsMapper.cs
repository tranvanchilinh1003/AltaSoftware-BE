using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Mappers
{
    public class TeachingAssignmentsMapper : Profile
    {
        public TeachingAssignmentsMapper()
        {
            CreateMap<TeachingAssignment, TeachingAssignmentsResponse>()
                .ForMember(dest => dest.User, opt => opt.MapFrom(src =>
                    src.User != null ? new TeachingAssignmentsResponse.TeachingAssignmentsUserResponse
                    {
                        Id = src.User.Id,
                        Code = src.User.Code,
                        FullName = src.User.FullName
                    } : null))

                .ForMember(dest => dest.Class, opt => opt.MapFrom(src =>
                    src.Class != null ? new TeachingAssignmentsResponse.TeachingAssignmentsClassResponse
                    {
                        Id = src.Class.Id,
                        Name = src.Class.Name
                    } : null))

                .ForMember(dest => dest.Subject, opt => opt.MapFrom(src =>
                    src.Subject != null ? new TeachingAssignmentsResponse.TeachingAssignmentsSubjectResponse
                    {
                        Id = src.Subject.Id,
                        Name = src.Subject.Name
                    } : null))

                .ForMember(dest => dest.SubjectGroup, opt => opt.MapFrom(src =>
                    src.Subject != null && src.Subject.SubjectGroup != null ? new SubjectGroupResponse
                    {
                        Id = src.Subject.SubjectGroup.Id,
                        Name = src.Subject.SubjectGroup.Name

                    } : null))

                .ForMember(dest => dest.Topics, opt => opt.MapFrom(src =>
                    src.Topics != null ? new TeachingAssignmentsResponse.TeachingAssignmentsTopicResponse
                    {
                        Id = src.Topics.Id,
                        Name = src.Topics.Name
                    } : null))

                .ForMember(dest => dest.Sessions, opt => opt.MapFrom(src =>
                        src.Sessions != null
                        ? src.Sessions.Select(session => new TeachingAssignmentsResponse.TeachingAssignmentsSessionsResponse
                        {
                            Id = session.Id,
                            Name = session.Name
                        }).ToList()
                        : new List<TeachingAssignmentsResponse.TeachingAssignmentsSessionsResponse>()
                    ))
                .ForMember(dest => dest.Semester, opt => opt.MapFrom(src =>
                src.Semester != null ? new TeachingAssignmentsResponse.TeachingAssignmentsSemesterResponse
                {
                    Id = src.Semester.Id,
                    Name = src.Semester.Name,
                    AcademicYear = src.Semester.AcademicYear != null ? new AcademicYearResponse
                    {
                        Id = src.Semester.AcademicYear.Id,
                        StartTime = (DateTime)src.Semester.AcademicYear.StartTime,
                        EndTime = (DateTime)src.Semester.AcademicYear.EndTime,
                    } : null
                } : null
            ));
            CreateMap<TeachingAssignmentsRequest, TeachingAssignment>();
        }
    }
}
