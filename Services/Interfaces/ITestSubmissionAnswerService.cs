using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Services.Interfaces
{
    public interface ITestSubmissionAnswerService
    {
        ApiResponse<ICollection<TestSubmissionAnswerResponse>> GetTestSubmissionAnswers(int page, int pageSize, string search, string sortColumn, string sortOrder);
        ApiResponse<TestSubmissionAnswerResponse> GetTestSubmissionAnswerById(int id);
        Task<ApiResponse<TestSubmissionAnswerResponse>> CreateTestSubmissionAnswer(TestSubmissionAnswerRequest request, List<IFormFile> attachments);
        Task<ApiResponse<TestSubmissionAnswerResponse>> UpdateTestSubmissionAnswer(int id, TestSubmissionAnswerRequest request, List<IFormFile> attachments);
        ApiResponse<TestSubmissionAnswerResponse> DeleteTestSubmissionAnswer(int id);
        ApiResponse<ICollection<TestSubmissionAnswerResponse>> GetAnswersByTestId(long testId, int pageNumber, int pageSize);
    }
}
