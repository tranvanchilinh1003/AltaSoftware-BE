using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.DTOs.Requests;
using System.Collections.Generic;
using ISC_ELIB_SERVER.Models;
using System.Security.Claims;

namespace ISC_ELIB_SERVER.Services.Interfaces
{
    public interface IStudentScoreService
    {
        ApiResponse<StudentScoreResponse> GetStudentScoreById(int id);
        ApiResponse<StudentScoreResponse> CreateStudentScore(StudentScoreRequest studentScoreRequest);
        ApiResponse<StudentScoreResponse> UpdateStudentScore(int id, StudentScoreRequest studentScoreRequest);
        ApiResponse<StudentScore> DeleteStudentScore(int id);
        Task<ApiResponse<StudentScoreDashboardResponse>> ViewStudentDashboardScores(int? academicYearId, int? classId, int? subjectId);

    }
}
