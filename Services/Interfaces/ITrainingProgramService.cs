using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Services.Interfaces
{
    public interface ITrainingProgramService
    {
        ApiResponse<ICollection<TrainingProgramsResponse>> GetTrainingPrograms(int? page, int? pageSize, string? search, string? sortColumn, string? sortOrder);
        ApiResponse<TrainingProgramsResponse> GetTrainingProgramsById(long id, long teacherId);
        ApiResponse<ICollection<TrainingProgramsResponse>> GetTrainingProgramsByTeacherId(long teacherID, string? search);
        ApiResponse<TrainingProgramsResponse> CreateTrainingPrograms(TrainingProgramsRequest trainingProgramsRequest);
        ApiResponse<TrainingProgramsResponse> UpdateTrainingPrograms(long id, TrainingProgramsRequest trainingProgramsRequest);
        ApiResponse<TrainingProgram> DeleteTrainingPrograms(long id, long teacherId);
    }
}
