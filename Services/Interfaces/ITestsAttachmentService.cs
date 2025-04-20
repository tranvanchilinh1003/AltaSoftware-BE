using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Services.Interfaces
{
    public interface ITestsAttachmentService
    {
        ApiResponse<ICollection<TestsAttachmentResponse>> GetTestsAttachments(int page, int pageSize, string search, string sortColumn, string sortOrder);
        ApiResponse<TestsAttachmentResponse> GetTestsAttachmentById(long id);
        ApiResponse<TestsAttachmentResponse> CreateTestsAttachment(TestsAttachmentRequest request);
        ApiResponse<TestsAttachmentResponse> UpdateTestsAttachment(long id, TestsAttachmentRequest request);
        ApiResponse<TestsAttachmentResponse> DeleteTestsAttachment(long id);
    }
}
