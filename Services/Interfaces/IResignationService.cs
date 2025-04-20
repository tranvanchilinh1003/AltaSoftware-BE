using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Services.Interfaces
{
    public interface IResignationService
    {
        ApiResponse<ICollection<ResignationResponse>> GetResignation(int page, int pageSize, string search, string sortColumn, string sortOrder);

        ApiResponse<ICollection<ResignationResponse>> GetResignationNoPaging();
        ApiResponse<ResignationResponse> GetResignationById(long id);
        ApiResponse<ICollection<ResignationResponse>> GetResignationByTeacherId(long id);
        ApiResponse<ResignationResponse> CreateResignation(ResignationRequest Resignation_AddRequest);
        ApiResponse<Resignation> UpdateResignation(long id, ResignationRequest Resignation_UpdateRequest);
        ApiResponse<Resignation> DeleteResignation(long id);
    }
}
