using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.Services.Interfaces;
using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Responses.ISC_ELIB_SERVER.DTOs.Responses;

namespace ISC_ELIB_SERVER.Services
{
    public class ScoreTypeService : IScoreTypeService
    {
        private readonly IScoreTypeRepo _repository;
        private readonly IMapper _mapper;

        public ScoreTypeService(IScoreTypeRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        

        public ApiResponse<ScoreTypeResponse> GetScoreTypeById(int id)
        {
            var scoreType = _repository.GetScoreTypeById(id);
            return scoreType != null
                ? ApiResponse<ScoreTypeResponse>.Success(_mapper.Map<ScoreTypeResponse>(scoreType))
                : ApiResponse<ScoreTypeResponse>.NotFound($"Không có dữ liệu");
        }

        public ApiResponse<ScoreTypeResponse> CreateScoreType(ScoreTypeRequest scoreTypeRequest)
        {
            var existing = _repository.GetScoreTypes().FirstOrDefault(st => st.Name.ToLower() == scoreTypeRequest.Name.ToLower());
            if (existing != null)
            {
                return ApiResponse<ScoreTypeResponse>.Conflict("Tên loại điểm đã tồn tại");
            }

            var created = _repository.CreateScoreType(new ScoreType()
            {
                Name = scoreTypeRequest.Name,
                Weight = scoreTypeRequest.Weight,
                QtyScoreSemester1 = scoreTypeRequest.QtyScoreSemester1,
                QtyScoreSemester2 = scoreTypeRequest.QtyScoreSemester2
            });

            return ApiResponse<ScoreTypeResponse>.Success(_mapper.Map<ScoreTypeResponse>(created));
        }

        public ApiResponse<ScoreTypeResponse> UpdateScoreType(int id, ScoreTypeRequest scoreTypeRequest)
        {
            var existingScoreType = _repository.GetScoreTypeById(id);

            if (existingScoreType == null)
            {
                return ApiResponse<ScoreTypeResponse>.NotFound($"Không tìm thấy ScoreType với ID {id}");
            }

            var duplicate = _repository.GetScoreTypes()
                .FirstOrDefault(st => st.Name.ToLower() == scoreTypeRequest.Name.ToLower() && st.Id != id);

            if (duplicate != null)
            {
                return ApiResponse<ScoreTypeResponse>.Conflict("Tên loại điểm đã tồn tại");
            }

            existingScoreType.Name = scoreTypeRequest.Name;
            existingScoreType.Weight = scoreTypeRequest.Weight;
            existingScoreType.QtyScoreSemester1 = scoreTypeRequest.QtyScoreSemester1;
            existingScoreType.QtyScoreSemester2 = scoreTypeRequest.QtyScoreSemester2;

            var updated = _repository.UpdateScoreType(existingScoreType);

            return ApiResponse<ScoreTypeResponse>.Success(_mapper.Map<ScoreTypeResponse>(updated));
        }

        public ApiResponse<ScoreType> DeleteScoreType(int id)
        {
            var success = _repository.DeleteScoreType(id);
            return success
                ? ApiResponse<ScoreType>.Success()
                : ApiResponse<ScoreType>.NotFound("Không tìm thấy để xóa");
        }

        public ApiResponse<ICollection<ScoreTypeResponse>> GetScoreTypes(
            int? page, int? pageSize, string? search, string? sortColumn, string? sortOrder)
        {
            var query = _repository.GetScoreTypes().AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(st => st.Name.ToLower().Contains(search.ToLower()));
            }

            query = sortColumn switch
            {
                "Id" => (sortOrder?.ToLower() == "desc") ? query.OrderByDescending(st => st.Id) : query.OrderBy(st => st.Id),
                _ => query.OrderBy(st => st.Id)
            };

            int totalRecords = query.Count();

            if (page.HasValue && pageSize.HasValue)
            {
                query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }

            var result = query.ToList();
            var response = _mapper.Map<ICollection<ScoreTypeResponse>>(result);

            return result.Any()
                ? ApiResponse<ICollection<ScoreTypeResponse>>.Success(response, page, pageSize, totalRecords)
                : ApiResponse<ICollection<ScoreTypeResponse>>.NotFound("Không có dữ liệu");
        }

    }
}
