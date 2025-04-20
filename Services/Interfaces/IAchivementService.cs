using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Services.Interfaces
{
    public interface IAchivementService
    {
        ApiResponse<ICollection<AchivementResponse>> GetAchivements(int page, int pageSize, string search, string sortColumn, string sortOrder);
        ApiResponse<ICollection<AchivementResponse>> GetAwards(int page, int pageSize, string search, string sortColumn, string sortOrder, int typeId = 0);
        ApiResponse<ICollection<AchivementResponse>> GetDisciplines(int page, int pageSize, string search, string sortColumn, string sortOrder, int typeId = 1);
        ApiResponse<AchivementResponse> GetAchivementById(int id);
        ApiResponse<AchivementResponse> CreateAchivement(AchivementRequest achivementRequest);
        ApiResponse<AchivementResponse> UpdateAchivement(int id, AchivementRequest achivementRequest);
        ApiResponse<bool> DeleteAchivement(int id);
    }
}
