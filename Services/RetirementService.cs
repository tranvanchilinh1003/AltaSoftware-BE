using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;
using ISC_ELIB_SERVER.Services.Interfaces;
using System.Linq;

namespace ISC_ELIB_SERVER.Services
{

    public class RetirementService : IRetirementService
    {
        private readonly RetirementRepo _repository;
        private readonly IMapper _mapper;
        private readonly isc_dbContext _context;
        private readonly CloudinaryService _cloudinaryService;

        public RetirementService(RetirementRepo repository, IMapper mapper, isc_dbContext context, CloudinaryService cloudinaryService)
        {
            _repository = repository;
            _mapper = mapper;
            _context = context;
            _cloudinaryService = cloudinaryService;

        }

        public ApiResponse<RetirementResponse> CreateRetirement(RetirementRequest Retirement_AddRequest)
        {
            if (Retirement_AddRequest.Date.HasValue)
            {
                // Chuyển sang DateTime có Kind Unspecified
                Retirement_AddRequest.Date = DateTime.SpecifyKind(Retirement_AddRequest.Date.Value, DateTimeKind.Unspecified);
            }
            var existingRetirement = _repository.GetRetirementByTeacherId(Retirement_AddRequest.TeacherId);
            if (existingRetirement != null && existingRetirement.Any())
            {
                return ApiResponse<RetirementResponse>.BadRequest("Giáo viên đã có trong danh sách");
            }
            Retirement_AddRequest.Attachment = _cloudinaryService.UploadBase64Async(Retirement_AddRequest.Attachment).Result;
            var retirement = _mapper.Map<Retirement>(Retirement_AddRequest);
            var created = _repository.CreateRetirement(retirement);
            return ApiResponse<RetirementResponse>.Success(_mapper.Map<RetirementResponse>(created));
        }

        public ApiResponse<Retirement> DeleteRetirement(long id)
        {
            var success = _repository.DeleteRetirement(id);
            return success
                ? ApiResponse<Retirement>.Success()
                : ApiResponse<Retirement>.NotFound("Không tìm thấy dữ liệu để xóa");
        }

        public ApiResponse<ICollection<RetirementResponse>> GetRetirement(int page, int pageSize, string search, string sortColumn, string sortOrder)
        {
            var query = _repository.GetRetirement().AsQueryable();


            query = sortColumn switch
            {
                "Id" => sortOrder.ToLower() == "desc" ? query.OrderByDescending(us => us.Id) : query.OrderBy(us => us.Id),
                _ => query.OrderBy(us => us.Id)
            };

            var result = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var response = _mapper.Map<ICollection<RetirementResponse>>(result);

            return result.Any()
                ? ApiResponse<ICollection<RetirementResponse>>.Success(response)
                : ApiResponse<ICollection<RetirementResponse>>.NotFound("Không có dữ liệu");
        }

        public ApiResponse<ICollection<RetirementResponse>> GetRetirementNoPaging()
        {
            var exsting = _repository.GetRetirement();
            var response = _mapper.Map<ICollection<RetirementResponse>>(exsting);
            return exsting.Any()
                ? ApiResponse<ICollection<RetirementResponse>>.Success(response)
                : ApiResponse<ICollection<RetirementResponse>>.NotFound("Không có dữ liệu");
        }


        public ApiResponse<RetirementResponse> GetRetirementById(long id)
        {
            var Retirement = _repository.GetRetirementById(id);
            return Retirement != null
                ? ApiResponse<RetirementResponse>.Success(_mapper.Map<RetirementResponse>(Retirement))
                : ApiResponse<RetirementResponse>.NotFound($"Không tìm thấy trạng thái nghỉ hưu  #{id}");
        }

        public ApiResponse<ICollection<RetirementResponse>> GetRetirementByTeacherId(long id)
        {

            var resignations = _repository.GetRetirementByTeacherId(id);

            if (resignations == null || !resignations.Any())
            {
                return ApiResponse<ICollection<RetirementResponse>>.NotFound("Không có dữ liệu");
            }

            var response = resignations.Select(r => new RetirementResponse
            {
                Id = r.Id,
                TeacherId = (int)r.TeacherId,
                Date = r.Date,
                Note = r.Note,
                Attachment = r.Attachment,
                Status = r.Status
            }).ToList();

            return ApiResponse<ICollection<RetirementResponse>>.Success(response);
        }
        public ApiResponse<Retirement> UpdateRetirement(long id, RetirementRequest retirementRequest)
        {
            try
            {
                if (retirementRequest.Date.HasValue)
                {
                    // Chuyển sang DateTime có Kind Unspecified
                    retirementRequest.Date = DateTime.SpecifyKind(retirementRequest.Date.Value, DateTimeKind.Unspecified);
                }

                // Kiểm tra sự tồn tại của TeacherId và LeadershipId
                if (!_context.TeacherInfos.Any(t => t.Id == retirementRequest.TeacherId && t.Active))
                {
                    return ApiResponse<Retirement>.NotFound("Giảng viên không tồn tại");
                }

                if (!_context.Users.Any(u => u.Id == retirementRequest.LeadershipId && u.Active))
                {
                    return ApiResponse<Retirement>.NotFound("Người dùng không tồn tại");
                }
                retirementRequest.Attachment = _cloudinaryService.UploadBase64Async(retirementRequest.Attachment).Result;
                var updated = _repository.UpdateRetirement(id, retirementRequest);
                return updated != null
                    ? ApiResponse<Retirement>.Success(updated)
                    : ApiResponse<Retirement>.NotFound("Không tìm thấy trạng thái nghỉ hưu để cập nhật");
            }
            catch (Exception ex)
            {
                // Ghi log lỗi nếu cần thiết
                // _logger.LogError(ex, "Error updating retirement");

                return ApiResponse<Retirement>.Fail($"Đã xảy ra lỗi khi cập nhật trạng thái nghỉ hưu: {ex.Message}");
            }
        }

        public ApiResponse<Retirement> UpdateRetirementByTeacherId(long id, RetirementRequest RetirementRequest)
        {
            try
            {
                if (RetirementRequest.Date.HasValue)
                {
                    // Chuyển sang DateTime có Kind Unspecified
                    RetirementRequest.Date = DateTime.SpecifyKind(RetirementRequest.Date.Value, DateTimeKind.Unspecified);
                }

                // Kiểm tra sự tồn tại của TeacherId và LeadershipId
                if (!_context.TeacherInfos.Any(t => t.Id == RetirementRequest.TeacherId && t.Active))
                {
                    return ApiResponse<Retirement>.NotFound("Giảng viên không tồn tại");
                }

                if (!_context.Users.Any(u => u.Id == RetirementRequest.LeadershipId && u.Active))
                {
                    return ApiResponse<Retirement>.NotFound("Người dùng không tồn tại");
                }
                RetirementRequest.Attachment = _cloudinaryService.UploadBase64Async(RetirementRequest.Attachment).Result;
                var updated = _repository.UpdateRetirementByTeacherId(id, RetirementRequest);
                return updated != null
                    ? ApiResponse<Retirement>.Success(updated)
                    : ApiResponse<Retirement>.NotFound("Không tìm thấy trạng thái nghỉ hưu để cập nhật");
            }
            catch (Exception ex)
            {
                // Ghi log lỗi nếu cần thiết
                // _logger.LogError(ex, "Error updating retirement");

                return ApiResponse<Retirement>.Fail($"Đã xảy ra lỗi khi cập nhật trạng thái nghỉ hưu: {ex.Message}");
            }
        }
    }

}
