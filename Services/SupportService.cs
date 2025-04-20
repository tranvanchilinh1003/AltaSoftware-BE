using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.DTOs.Requests;
using AutoMapper;
using ISC_ELIB_SERVER.Services.Interfaces;
using ISC_ELIB_SERVER.DTOs.Responses.ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Enums;

namespace ISC_ELIB_SERVER.Services
{

    public class SupportService : ISupportService
    {
        private readonly SupportRepo _repository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public SupportService(SupportRepo repository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public ApiResponse<ICollection<SupportResponse>> GetSupports(int? page, int? pageSize, string? sortColumn, string? sortOrder)
        {
            var query = _repository.GetSupports().AsQueryable();

            sortColumn = string.IsNullOrWhiteSpace(sortColumn) ? "Id" : sortColumn;
            sortOrder = sortOrder?.ToLower() ?? "asc";

            query = sortColumn switch
            {
                "Id" => sortOrder == "desc" ? query.OrderByDescending(s => s.Id) : query.OrderBy(s => s.Id),
                _ => query.OrderBy(s => s.Id)
            };

            int totalRecords = query.Count();

            if (page.HasValue && pageSize.HasValue)
            {
                query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }

            var result = query.ToList();
            var response = _mapper.Map<ICollection<SupportResponse>>(result);

            return result.Any()
                ? ApiResponse<ICollection<SupportResponse>>.Success(response, page, pageSize, totalRecords)
                : ApiResponse<ICollection<SupportResponse>>.NotFound("Không có dữ liệu");
        }


        public ApiResponse<SupportResponse> GetSupportById(int id)
        {
            var Support = _repository.GetSupportById(id);
            return Support != null ? ApiResponse<SupportResponse>.Success(_mapper.Map<SupportResponse>(Support)) : ApiResponse<SupportResponse>.NotFound($"Không tìm thấy thông báo #{id}");
        }
        private int? GetUserIdFromToken()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst("Id")?.Value;
            return int.TryParse(userIdClaim, out var userId) ? userId : (int?)null;
        }
        public ApiResponse<SupportResponse> CreateSupport(SupportRequest supportRequest)
        {
            var userId = GetUserIdFromToken();
            if (userId == null)
                return ApiResponse<SupportResponse>.Unauthorized("Người dùng chưa đăng nhập.");

            if (string.IsNullOrWhiteSpace(supportRequest.Title) || string.IsNullOrWhiteSpace(supportRequest.Content))
            {
                return ApiResponse<SupportResponse>.BadRequest("Tiêu đề và nội dung không được để trống.");
            }

            if (!Enum.IsDefined(typeof(SupportType), supportRequest.Type))
            {
                return ApiResponse<SupportResponse>.BadRequest("Loại hỗ trợ không hợp lệ.");
            }

            var support = _mapper.Map<Support>(supportRequest);
            support.UserId = userId.Value;
            support.CreateAt = DateTime.Now;
            support.Type = (int?)(SupportType)supportRequest.Type;

            _repository.CreateSupport(support);

            var response = _mapper.Map<SupportResponse>(support);

            return ApiResponse<SupportResponse>.Success(response);
        }



        public ApiResponse<SupportResponse> UpdateSupport(int id, SupportRequest supportRequest)
        {
            var support = _repository.GetSupportById(id);
            if (support == null)
            {
                return ApiResponse<SupportResponse>.NotFound("Không tìm thấy yêu cầu hỗ trợ để cập nhật.");
            }

            support.Title = supportRequest.Title;
            support.Content = supportRequest.Content;

            if (!Enum.IsDefined(typeof(SupportType), supportRequest.Type))
            {
                return ApiResponse<SupportResponse>.BadRequest("Loại hỗ trợ không hợp lệ.");
            }
            support.Type = (int)supportRequest.Type;

            var updatedSupport = _repository.UpdateSupport(support);

            return ApiResponse<SupportResponse>.Success(_mapper.Map<SupportResponse>(updatedSupport));
        }


        public ApiResponse<bool> DeleteSupport(int id)
        {
            var success = _repository.DeleteSupport(id);
            return success
                ? ApiResponse<bool>.Success(true)
                : ApiResponse<bool>.NotFound("Không tìm thấy yêu cầu hỗ trợ để xóa.");
        }
    }
}
