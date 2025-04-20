using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Mappers
{
    public class TrainingProgramsMapper :Profile
    {
        public TrainingProgramsMapper()
        {
            // us - res
            CreateMap<TrainingProgram, TrainingProgramsResponse>();
            // res - us
            CreateMap<TrainingProgramsRequest, TrainingProgram>();
        }
    }
}
