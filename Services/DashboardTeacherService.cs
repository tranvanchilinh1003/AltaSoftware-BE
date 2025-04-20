using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Repositories;
using ISC_ELIB_SERVER.Services.Interfaces;

namespace ISC_ELIB_SERVER.Services
{
    public class DashboardTeacherService : IDashboardTeacherService
    {
        private readonly DashboardTeacherRepo _dashboardTeacherRepo;

        public DashboardTeacherService(DashboardTeacherRepo dashboardTeacherRepo)
        {
            _dashboardTeacherRepo = dashboardTeacherRepo;
        }

        public ApiResponse<DashboardOverviewResponse> GetDashboardOverview(int teacherId)
        {
            var data = _dashboardTeacherRepo.GetDashboardOverview(teacherId);
            return data != null
                ? ApiResponse<DashboardOverviewResponse>.Success(data)
                : ApiResponse<DashboardOverviewResponse>.NotFound("Không tìm thấy dữ liệu tổng quan");
        }

        public ApiResponse<StudentStatisticsResponse> GetStudentStatistics(int teacherId)
        {
            var data = _dashboardTeacherRepo.GetStudentStatistics(teacherId);
            return data != null
                ? ApiResponse<StudentStatisticsResponse>.Success(data)
                : ApiResponse<StudentStatisticsResponse>.NotFound("Không tìm thấy dữ liệu thống kê học sinh");
        }
    }
}
