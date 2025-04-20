using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Services.Interfaces
{
    public interface ITestsSubmissionService
    {
        ApiResponse<ICollection<TestsSubmissionResponse>> GetTestsSubmissiones(int page, int pageSize, string search, string sortColumn, string sortOrder);
        ApiResponse<TestsSubmissionResponse> GetTestsSubmissionById(long id);
        //ApiResponse<TestsSubmissionResponse> GetTestsSubmissionByName(string name);
        //ApiResponse<TestsSubmissionResponse> GetTestsSubmissionByTestId(long testId);
        Task<ApiResponse<List<TestsSubmissionResponse>>> GetByTestIdAsync(int testId);
        Task<ApiResponse<TestsSubmissionResponse>> CreateTestsSubmission(TestsSubmissionRequest request, List<TestSubmissionAnswerRequest> answerRequests);
        Task<ApiResponse<TestsSubmissionResponse>> UpdateTestsSubmission(int submissionId, TestsSubmissionRequest request, List<TestSubmissionAnswerRequest>? answerRequests);
        ApiResponse<TestsSubmission> DeleteTestsSubmission(long id);
    }
}
