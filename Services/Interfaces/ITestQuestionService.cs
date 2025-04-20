using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Services.Interfaces
{
    public interface ITestQuestionService
    {
        ApiResponse<ICollection<TestQuestionResponse>> GetTestQuestiones(int page, int pageSize, string search, string sortColumn, string sortOrder);
        ApiResponse<TestQuestionResponse> GetTestQuestionById(long id);
        //ApiResponse<TestQuestionResponse> GetTestQuestionByName(string name);
        ApiResponse<TestQuestionResponse> CreateTestQuestion(TestQuestionRequest TestQuestionRequest);
        ApiResponse<TestQuestionResponse> UpdateTestQuestion(long id, TestQuestionRequest TestQuestion);
        ApiResponse<TestQuestion> DeleteTestQuestion(long id);
    }
}
