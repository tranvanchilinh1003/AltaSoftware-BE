using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Services.Interfaces
{
    public interface ITestService
    {
        ApiResponse<ICollection<TestResponse>> GetTestes(int? page, int? pageSize, string? search, string? sortColumn, string? sortOrder);
        ApiResponse<ICollection<TestByStudentResponse>> GetTestesByStudent(int? page, int? pageSize, string? search, string? sortColumn, string? sortOrder, int status, long? subjectGroupId, long? gradeLevelsId, string? date, string? userId);
        ApiResponse<TestResponse> GetTestById(long id);
        ApiResponse<TestResponse> GetTestByName(string name);
        ApiResponse<TestResponse> CreateTest(TestRequest TestRequest, string? userId);
        ApiResponse<TestResponse> UpdateTest(long id, TestRequest Test, string? userId);
        ApiResponse<Test> DeleteTest(long id);
    }
}
