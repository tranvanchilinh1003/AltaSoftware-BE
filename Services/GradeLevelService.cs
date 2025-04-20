using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.DTOs.Requests;
using AutoMapper;
using ISC_ELIB_SERVER.Services.Interfaces;

namespace ISC_ELIB_SERVER.Services
{

    public class GradeLevelService : IGradeLevelService
    {
        private readonly GradeLevelRepo _repository;
        private readonly IMapper _mapper;

        public GradeLevelService(GradeLevelRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public ApiResponse<ICollection<GradeLevelResponse>> GetGradeLevels(int? page, int? pageSize, string? sortColumn, string? sortOrder)
        {
            var query = _repository.GetGradeLevels().AsQueryable();

            query = sortColumn switch
            {
                "Id" => sortOrder.ToLower() == "desc" ? query.OrderByDescending(us => us.Id) : query.OrderBy(us => us.Id),
                _ => query.OrderBy(ay => ay.Id)
            };


            if (page.HasValue && pageSize.HasValue)
            {
                query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }

            var result = query.ToList();

            var response = _mapper.Map<ICollection<GradeLevelResponse>>(result);

            return result.Any() ? ApiResponse<ICollection<GradeLevelResponse>>.Success(response, page, pageSize, _repository.GetGradeLevels().Count) : ApiResponse<ICollection<GradeLevelResponse>>.NotFound("Không có dữ liệu");
        }

        public ApiResponse<ICollection<GradeLevelResponse>> GetGradeLevelsByAyAndSc(int? page, int? pageSize, string? sortColumn, string? sortOrder, string schoolName, int? startYear, int? endYear)
        {
            var query = _repository.GetGradeLevels(schoolName, startYear, endYear).AsQueryable();

            query = sortColumn switch
            {
                "Id" => sortOrder.ToLower() == "desc" ? query.OrderByDescending(us => us.Id) : query.OrderBy(us => us.Id),
                _ => query.OrderBy(ay => ay.Id)
            };


            if (page.HasValue && pageSize.HasValue)
            {
                query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }

            var result = query.ToList();

            var response = _mapper.Map<ICollection<GradeLevelResponse>>(result);

            return result.Any() ? ApiResponse<ICollection<GradeLevelResponse>>.Success(response) : ApiResponse<ICollection<GradeLevelResponse>>.NotFound("Không có dữ liệu");
        }

        public ApiResponse<GradeLevelResponse> GetGradeLevelById(long id)
        {
            var GradeLevel = _repository.GetGradeLevelById(id);
            return GradeLevel != null
                ? ApiResponse<GradeLevelResponse>.Success(_mapper.Map<GradeLevelResponse>(GradeLevel))
                : ApiResponse<GradeLevelResponse>.NotFound($"Không tìm thấy khoa - khối #{id}");
        }

        public ApiResponse<object> GetClassOfGradeLevel(long id)
        {
            var GradeLevel = _repository.GetClassesByGradeLevel(id);
            return GradeLevel != null
                ? ApiResponse<object>.Success(GradeLevel)
                : ApiResponse<object>.NotFound($"Không có dữ liệu");
        }

        public ApiResponse<GradeLevelResponse> CreateGradeLevel(GradeLevelRequest GradeLevelRequest)
        {
            var ListGradeLevel = _repository.GetGradeLevels().Where(item => item.Active);

            if (ListGradeLevel.Any(item => item.Code.Equals(GradeLevelRequest.Code)))
            {
                return ApiResponse<GradeLevelResponse>.BadRequest("Mã khoa - khối đã tồn tại");
            }

            if (ListGradeLevel.Any(item => item.Name.Equals(GradeLevelRequest.Name)))
            {
                return ApiResponse<GradeLevelResponse>.BadRequest("Tên khoa - khối đã tồn tại");
            }

            var newGradeLevel = new GradeLevel
            {
                Code = GradeLevelRequest.Code,
                Name = GradeLevelRequest.Name,
                TeacherId = GradeLevelRequest.TeacherId
            };

            try
            {
                var created = _repository.CreateGradeLevel(newGradeLevel);
                return ApiResponse<GradeLevelResponse>.Success(_mapper.Map<GradeLevelResponse>(created));
            }
            catch (Exception ex)
            {
                return ApiResponse<GradeLevelResponse>.BadRequest($"Lỗi: {ex.Message}, kiểm tra lại TeacherId");
            }
        }

        public ApiResponse<GradeLevelResponse> UpdateGradeLevel(long id, GradeLevelRequest GradeLevelRequest)
        {
            var existing = _repository.GetGradeLevelById(id);
            if (existing == null)
            {
                return ApiResponse<GradeLevelResponse>.NotFound($"Không tìm thấy khoa - khối #{id}");
            }

            var ListGradeLevel = _repository.GetGradeLevels().Where(item => item.Active);

            if (ListGradeLevel.Any(item => item.Code.Equals(GradeLevelRequest.Code) && item.Id != id))
            {
                return ApiResponse<GradeLevelResponse>.BadRequest("Mã khoa - khối đã tồn tại");
            }

            if (ListGradeLevel.Any(item => item.Name.Equals(GradeLevelRequest.Name) && item.Id != id))
            {
                return ApiResponse<GradeLevelResponse>.BadRequest("Tên khoa - khối đã tồn tại");
            }

            existing.Code = GradeLevelRequest.Code;
            existing.Name = GradeLevelRequest.Name;
            existing.TeacherId = GradeLevelRequest.TeacherId;
            try
            {


                var updated = _repository.UpdateGradeLevel(existing);
                return ApiResponse<GradeLevelResponse>.Success(_mapper.Map<GradeLevelResponse>(updated));
            }
            catch (Exception ex)
            {
                return ApiResponse<GradeLevelResponse>.BadRequest("Lỗi....");
            }

        }

        public ApiResponse<object> DeleteGradeLevel(long id)
        {
            var success = _repository.DeleteGradeLevel(id);
            return success ? ApiResponse<object>.Success() : ApiResponse<object>.NotFound($"Không tìm thấy khoa - khối #{id} để xóa");
        }
    }

}
