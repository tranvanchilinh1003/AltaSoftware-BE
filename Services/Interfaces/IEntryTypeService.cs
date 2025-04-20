using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;

namespace ISC_ELIB_SERVER.Services
{
    public interface IEntryTypeService
    {
        ApiResponse<ICollection<EntryTypeResponse>> GetEntryTypes(int page, int pageSize, string search, string sortColumn, string sortOrder);
        ApiResponse<EntryTypeResponse> GetEntryTypeById(long id);
        ApiResponse<EntryTypeResponse> CreateEntryType(EntryTypeRequest entryTypeRequest);
        ApiResponse<EntryTypeResponse> UpdateEntryType(long id, EntryTypeRequest entryTypeRequest);
        ApiResponse<object> DeleteEntryType(long id);
    }
}
