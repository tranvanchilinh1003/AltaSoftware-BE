using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.DTOs.Requests;
using AutoMapper;
using ISC_ELIB_SERVER.Services.Interfaces;
using System;
using Microsoft.EntityFrameworkCore;

namespace ISC_ELIB_SERVER.Services
{
    public class NotificationService : INotificationService
    {
        private readonly NotificationRepo _repository;
        private readonly UserRepo _userRepository;
        private readonly IMapper _mapper;

        public NotificationService(NotificationRepo repository, UserRepo userRepository, IMapper mapper)
        {
            _repository = repository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public ApiResponse<ICollection<NotificationResponse>> GetNotifications(int? page, int? pageSize, string? search, string? sortColumn, string? sortOrder)
        {
            var query = _repository.GetNotifications().AsQueryable();

            query = query.Where(us => us.Active == true);

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(us => us.Title.ToLower().Contains(search.ToLower()));
            }

            query = sortColumn switch
            {
                "CreateAt" => sortOrder.ToLower() == "desc" ? query.OrderByDescending(us => us.CreateAt) : query.OrderBy(us => us.CreateAt),
                "Id" => sortOrder.ToLower() == "desc" ? query.OrderByDescending(us => us.Id) : query.OrderBy(us => us.Id),
                _ => query.OrderBy(us => us.Id)
            };

            int totalItems = query.Count();

            if (page.HasValue && pageSize.HasValue)
            {
                query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }

            var result = query.ToList();

            var response = _mapper.Map<ICollection<NotificationResponse>>(result);

            return result.Any() ? ApiResponse<ICollection<NotificationResponse>>
            .Success(response, page, pageSize, totalItems)
            : ApiResponse<ICollection<NotificationResponse>>.NotFound("Không có dữ liệu");
        }

        public ApiResponse<NotificationResponse> GetNotificationById(long id)
        {
            var notification = _repository.GetNotificationById(id);
            return (notification != null && (notification.Active == true))
                ? ApiResponse<NotificationResponse>.Success(_mapper.Map<NotificationResponse>(notification))
                : ApiResponse<NotificationResponse>.NotFound($"Không tìm thông báo #{id}");
        }

        public ApiResponse<NotificationResponse> CreateNotification(NotificationRequest notificationRequest, int senderId)
        {
            bool senderExists = CheckUserExists(senderId);

            if (!senderExists)
            {
                return ApiResponse<NotificationResponse>.NotFound("Người gửi không tồn tại.");
            }


            bool userExists = CheckUserExists(notificationRequest.UserId);

            if (!userExists)
            {
                return ApiResponse<NotificationResponse>.NotFound("Người nhận không tồn tại.");
            }

            var created = _repository.CreateNotification(new Notification()
            {
                Title = notificationRequest.Title,
                Content = notificationRequest.Content,
                Type = notificationRequest.Type,
                SenderId = senderId,
                UserId = notificationRequest.UserId,
                Active = true,
                CreateAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified)
            });

            return ApiResponse<NotificationResponse>.Success(_mapper.Map<NotificationResponse>(created));
        }


        public ApiResponse<NotificationResponse> UpdateNotification(long id, NotificationRequest notificationRequest, int senderId)
        {
            var existingNotification = _repository.GetNotificationById(id);
            if (existingNotification == null || existingNotification.Active == false)
            {
                return ApiResponse<NotificationResponse>.NotFound("Không tìm thấy thông báo.");
            }

            if (!CheckUserExists(senderId))
            {
                return ApiResponse<NotificationResponse>.NotFound("Người gửi không tồn tại.");
            }

            if (!CheckUserExists(notificationRequest.UserId))
            {
                return ApiResponse<NotificationResponse>.NotFound("Người nhận không tồn tại.");
            }

            existingNotification.Title = notificationRequest.Title;
            existingNotification.Content = notificationRequest.Content;
            existingNotification.Type = notificationRequest.Type;
            existingNotification.SenderId = senderId;
            existingNotification.UserId = notificationRequest.UserId;
            //existingNotification.Active = false;
            existingNotification.CreateAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);

            _repository.UpdateNotification(existingNotification);
            return ApiResponse<NotificationResponse>.Success(_mapper.Map<NotificationResponse>(existingNotification));
        }

        public ApiResponse<Notification> DeleteNotification(long id)
        {
            var existingNotification = _repository.GetNotificationById(id);
            if (existingNotification == null)
            {
                return ApiResponse<Notification>.NotFound("Không tìm thấy thông báo.");
            }

            if (existingNotification.Active == false)
            {
                return ApiResponse<Notification>.Conflict("thông báo không tồn tại.");
            }

            existingNotification.Active = false;
            _repository.DeleteNotification(existingNotification);

            return ApiResponse<Notification>.Success();
        }

        public bool CheckUserExists(int? userId) 
        {
            if (!userId.HasValue) return false; 

            return _userRepository.GetUserById(userId.Value) != null;
        }
    }
}
