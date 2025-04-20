using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.DTOs.Requests;
using AutoMapper;
using ISC_ELIB_SERVER.Services.Interfaces;

namespace ISC_ELIB_SERVER.Services
{

    public class ChangeClassService : IChangeClassService
    {
        private readonly ChangeClassRepo _repository;
        private readonly IMapper _mapper;

        public ChangeClassService(ChangeClassRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public ApiResponse<ICollection<ChangeClassResponse>> GetChangeClasses(int page, int pageSize, string search, string sortColumn, string sortOrder)
        {
            var query = _repository.GetChangeClasses().AsQueryable();

            query = sortColumn switch
            {
                "Id" => sortOrder.ToLower() == "desc" ? query.OrderByDescending(us => us.Id) : query.OrderBy(us => us.Id),
                _ => query.OrderBy(us => us.Id)
            };

            var result = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var response = _mapper.Map<ICollection<ChangeClassResponse>>(result);

            return result.Any()
                ? ApiResponse<ICollection<ChangeClassResponse>>.Success(response)
                : ApiResponse<ICollection<ChangeClassResponse>>.NotFound("Không có dữ liệu");
        }

        public ApiResponse<ICollection<ChangeClassResponse>> GetChangeClassesNormal()
        {
            var result = _repository.GetChangeClasses();

            var response = _mapper.Map<ICollection<ChangeClassResponse>>(result);

            return result.Any()
                ? ApiResponse<ICollection<ChangeClassResponse>>.Success(response)
                : ApiResponse<ICollection<ChangeClassResponse>>.NotFound("Không có dữ liệu");
        }


        public ApiResponse<ChangeClassResponse> GetChangeClassById(long id)
        {
            var ChangeClass = _repository.GetChangeClassById(id);
            return ChangeClass != null
                ? ApiResponse<ChangeClassResponse>.Success(_mapper.Map<ChangeClassResponse>(ChangeClass))
                : ApiResponse<ChangeClassResponse>.NotFound($"Không tìm thấy thông tin tạm nghỉ của #{id}");
        }


        public ApiResponse<ChangeClassResponse> CreateChangeClass(ChangeClass_AddRequest request)
        {

                // Chuyển sang DateTime có Kind Unspecified
                request.ChangeClassDate = DateTime.SpecifyKind(request.ChangeClassDate, DateTimeKind.Unspecified);
            var ChangeClass = _mapper.Map<ChangeClass>(request);
            var created = _repository.CreateChangeClass(ChangeClass);
            return ApiResponse<ChangeClassResponse>.Success(_mapper.Map<ChangeClassResponse>(created));
        }

        public ApiResponse<ChangeClass> UpdateChangeClass(long id , ChangeClass_UpdateRequest request)
        {
                // Chuyển sang DateTime có Kind Unspecified
                request.ChangeClassDate = DateTime.SpecifyKind(request.ChangeClassDate, DateTimeKind.Unspecified);
            var ChangeClass = _mapper.Map<ChangeClass>(request);
            var updated = _repository.UpdateChangeClass(id ,ChangeClass);
            return updated != null
                ? ApiResponse<ChangeClass>.Success(updated)
                : ApiResponse<ChangeClass>.NotFound("Không tìm thấy trạng thái người dùng để cập nhật");
        }

        public ApiResponse<ChangeClass> DeleteChangeClass(long id)
        {
            var success = _repository.DeleteChangeClass(id);
            return success
                ? ApiResponse<ChangeClass>.Success()
                : ApiResponse<ChangeClass>.NotFound("Không tìm thấy trạng thái người dùng để xóa");
        }
    }

}
