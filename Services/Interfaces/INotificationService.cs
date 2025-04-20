using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;

namespace ISC_ELIB_SERVER.Services.Interfaces
{
    public interface INotificationService
    {
        ApiResponse<ICollection<NotificationResponse>> GetNotifications(int? page, int? pageSize, string? search, string? sortColumn, string? sortOrder);
        ApiResponse<NotificationResponse> GetNotificationById(long id);
        ApiResponse<NotificationResponse> CreateNotification(NotificationRequest notificationRequest, int senderID);
        ApiResponse<NotificationResponse> UpdateNotification(long id, NotificationRequest notificationRequest, int senderID);
        ApiResponse<Notification> DeleteNotification(long id);
    }
}
