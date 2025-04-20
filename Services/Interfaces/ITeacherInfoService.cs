using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;

namespace ISC_ELIB_SERVER.Services.Interfaces
{
    public interface ITeacherInfoService
    {
        ApiResponse<ICollection<TeacherInfoResponses>> GetTeacherInfos(int page, int pageSize, string search, string sortColumn, string sortOrder);
        ApiResponse<TeacherInfoResponses> GetTeacherInfoById(int id);
        ApiResponse<TeacherInfoResponses> GetTeacherInfoByCode(string code);
        ApiResponse<TeacherInfoResponses> CreateTeacherInfo(TeacherInfoRequest teacherInfoRequest);
        ApiResponse<TeacherInfoResponses> UpdateTeacherInfo(int id, TeacherInfoRequest teacherInfoRequest);
        ApiResponse<TeacherInfoResponses> DeleteTeacherInfo(int id);
    }
}
