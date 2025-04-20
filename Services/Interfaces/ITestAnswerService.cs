using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;

namespace ISC_ELIB_SERVER.Services
{
    public interface ITestAnswerService
    {
        ApiResponse<List<TestAnswerResponse>> GetAnswersByQuestion(int questionId);
        ApiResponse<TestAnswerResponse> CreateAnswer(TestAnswerRequest request);
        ApiResponse<TestAnswerResponse> UpdateAnswer(int id, TestAnswerRequest request);
        ApiResponse<bool> DeleteAnswer(int id);
    }
}
