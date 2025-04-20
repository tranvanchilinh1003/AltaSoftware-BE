using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models.Responses;

namespace ISC_ELIB_SERVER.Services.Interfaces
{
    public interface IExamService
    {
        ApiResponse<ICollection<ExamResponse>> GetExams(int page, int pageSize, string search, string sortColumn, string sortOrder);
        ApiResponse<ExamResponse> GetExamById(long id);
        ApiResponse<ICollection<ExamResponse>> GetExamByName(string name);
        ApiResponse<ExamResponse> CreateExam(ExamRequest request);
        ApiResponse<ExamResponse> UpdateExam(long id, ExamRequest request);
        ApiResponse<ExamResponse> DeleteExam(long id);
    }
}
