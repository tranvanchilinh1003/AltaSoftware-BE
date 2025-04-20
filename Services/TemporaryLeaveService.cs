using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.DTOs.Requests;
using AutoMapper;
using ISC_ELIB_SERVER.Utils;
using ISC_ELIB_SERVER.Services.Interfaces;

namespace ISC_ELIB_SERVER.Services
{


    public class TemporaryLeaveService : ITemporaryLeaveService
    {
        private readonly TemporaryLeaveRepo _repository;
        private readonly IMapper _mapper;

        public TemporaryLeaveService(TemporaryLeaveRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public ApiResponse<ICollection<TemporaryLeaveResponse>> GetTemporaryLeaves(int page, int pageSize, string search, string sortColumn, string sortOrder)
        {
            var query = _repository.GetTemporaryLeaves().AsQueryable();

            query = sortColumn switch
            {
                "Id" => sortOrder.ToLower() == "desc" ? query.OrderByDescending(us => us.Id) : query.OrderBy(us => us.Id),
                _ => query.OrderBy(us => us.Id)
            };

            var result = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var response = _mapper.Map<ICollection<TemporaryLeaveResponse>>(result);

            return result.Any()
                ? ApiResponse<ICollection<TemporaryLeaveResponse>>.Success(response)
                : ApiResponse<ICollection<TemporaryLeaveResponse>>.NotFound("Không có dữ liệu");
        }

        public ApiResponse<ICollection<TemporaryLeaveResponse>> GetTemporaryLeavesNormal()
        {
            var result = _repository.GetTemporaryLeaves();

            var response = _mapper.Map<ICollection<TemporaryLeaveResponse>>(result);

            return result.Any()
                ? ApiResponse<ICollection<TemporaryLeaveResponse>>.Success(response)
                : ApiResponse<ICollection<TemporaryLeaveResponse>>.NotFound("Không có dữ liệu");
        }


        public ApiResponse<TemporaryLeaveResponse> GetTemporaryLeaveById(long id)
        {
            var TemporaryLeave = _repository.GetTemporaryLeaveById(id);
            return TemporaryLeave != null
                ? ApiResponse<TemporaryLeaveResponse>.Success(_mapper.Map<TemporaryLeaveResponse>(TemporaryLeave))
                : ApiResponse<TemporaryLeaveResponse>.NotFound($"Không tìm thấy thông tin tạm nghỉ của #{id}");
        }


        public ApiResponse<TemporaryLeaveResponse> CreateTemporaryLeave(TemporaryLeave_AddRequest request)
        {
            if (request.Date.HasValue)
            {
                // Chuyển sang DateTime có Kind Unspecified
                request.Date = DateTime.SpecifyKind(request.Date.Value, DateTimeKind.Unspecified);
            }

            var temporaryLeave = _mapper.Map<TemporaryLeave>(request);
            var created = _repository.CreateTemporaryLeave(temporaryLeave);
            return ApiResponse<TemporaryLeaveResponse>.Success(_mapper.Map<TemporaryLeaveResponse>(created));
        }


        public ApiResponse<TemporaryLeave> UpdateTemporaryLeave(long id , TemporaryLeave_UpdateRequest request)
        {
            if (request.Date.HasValue)
            {
                // Chuyển sang DateTime có Kind Unspecified
                request.Date = DateTime.SpecifyKind(request.Date.Value, DateTimeKind.Unspecified);
            }
            var temporaryLeave = _mapper.Map<TemporaryLeave>(request);
            var updated = _repository.UpdateTemporaryLeave(id ,temporaryLeave);
            return updated != null
                ? ApiResponse<TemporaryLeave>.Success(updated)
                : ApiResponse<TemporaryLeave>.NotFound("Không tìm thấy trạng thái người dùng để cập nhật");
        }

        public ApiResponse<TemporaryLeave> DeleteTemporaryLeave(long id)
        {
            var success = _repository.DeleteTemporaryLeave2(id);
            return success
                ? ApiResponse<TemporaryLeave>.Success()
                : ApiResponse<TemporaryLeave>.NotFound("Không tìm thấy trạng thái người dùng để xóa");
        }
    }

}
