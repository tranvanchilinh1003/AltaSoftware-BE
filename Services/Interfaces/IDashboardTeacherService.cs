using ISC_ELIB_SERVER.DTOs.Responses;

public interface IDashboardTeacherService
{
    ApiResponse<DashboardOverviewResponse> GetDashboardOverview(int teacherId);
    ApiResponse<StudentStatisticsResponse> GetStudentStatistics(int teacherId);
}
