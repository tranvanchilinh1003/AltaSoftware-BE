using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Mappers
{
    public class AcademicYearMapper : Profile
    {
        public AcademicYearMapper()
        {
            CreateMap<Semester, SemesterAcademicYearResponse>();
            CreateMap<AcademicYear, AcademicYearResponse>()
            .ForMember(academicYear => academicYear.Name,
             opt => opt.MapFrom(src => $"{src.StartTime:yyyy}-{src.EndTime:yyyy}"));
            CreateMap<AcademicYearRequest, AcademicYear>();
        }
    }
}
