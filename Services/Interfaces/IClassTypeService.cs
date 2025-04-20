using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using System.Collections.Generic;

namespace ISC_ELIB_SERVER.Services
{
    public interface IClassTypeService
    {
        ApiResponse<ICollection<ClassTypeResponse>> GetClassTypes(
    int? page, int? pageSize, int? searchYear, string? searchName, string? sortColumn, string? sortOrder);
        ApiResponse<ClassTypeResponse> GetClassTypeById(int id);
        ApiResponse<ClassTypeResponse> CreateClassType(ClassTypeRequest classTypeRequest);
        ApiResponse<ClassTypeResponse> UpdateClassType(int id, ClassTypeRequest classTypeRequest);
        ApiResponse<bool> DeleteClassType(int id);
    }
}
