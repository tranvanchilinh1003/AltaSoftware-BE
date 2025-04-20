using System.Collections.Generic;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;

namespace ISC_ELIB_SERVER.Services
{
    public interface ITopicsFileService
    {
        ApiResponse<ICollection<TopicsFileResponse>> GetAll(int page, int pageSize, string search, string sortColumn, string sortOrder);
        ApiResponse<TopicsFileResponse> GetById(int id);
        ApiResponse<TopicsFileResponse> Create(TopicsFileRequest request);
        ApiResponse<TopicsFileResponse> Update(int id, TopicsFileRequest request);
        ApiResponse<string> Delete(int id);
    }
}
