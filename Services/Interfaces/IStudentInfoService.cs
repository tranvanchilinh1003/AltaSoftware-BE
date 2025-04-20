using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;

namespace ISC_ELIB_SERVER.Services.Interfaces
{
    public interface IStudentInfoService
    {
        ApiResponse<ICollection<StudentInfoResponses>> GetStudentInfos(int page, int pageSize, string search, string sortColumn, string sortOrder);
        ApiResponse<StudentInfoResponses> GetStudentInfoById(int userId);
        ApiResponse<StudentInfoResponses> CreateStudentInfo(StudentInfoRequest studentInfoRequest);
        ApiResponse<StudentInfoResponses> UpdateStudentInfo(int id, StudentInfoRequest studentInfoRequest);
        ApiResponse<StudentInfoResponses> DeleteStudentInfo(int id);

        // Thêm phương thức lấy danh sách học viên theo UserId
        ApiResponse<ICollection<StudentInfoResponses>> GetStudentsByUserId(int userId);

        // Thêm phương thức lấy danh sách học viên theo user figma
        ApiResponse<ICollection<StudentInfoUserResponse>> GetAllStudents();


        // Thêm phương thức lấy danh sách học viên theo lớp với thông tin đầy đủ
        ApiResponse<ICollection<StudentInfoClassResponse>> GetStudentsByClass(int classId);
    }
}
