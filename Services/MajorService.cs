using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;
using ISC_ELIB_SERVER.Services.Interfaces;

namespace ISC_ELIB_SERVER.Services
{
    public class MajorService : IMajorService
    {
        private readonly MajorRepo _repository;
        private readonly IMapper _mapper;

        public MajorService(MajorRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public ApiResponse<ICollection<MajorResponse>> GetMajor(int? page, int? pageSize, string? search, string? sortColumn, string? sortOrder)
        {
            var query = _repository.GetMajor().AsQueryable();

            query = query.Where(us => us.Active == true);

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(us => us.Name.ToLower().Contains(search.ToLower()));
            }

            query = sortColumn switch
            {
                "Name" => sortOrder.ToLower() == "desc" ? query.OrderByDescending(us => us.Name) : query.OrderBy(us => us.Name),
                "Id" => sortOrder.ToLower() == "desc" ? query.OrderByDescending(us => us.Id) : query.OrderBy(us => us.Id),
                _ => query.OrderBy(us => us.Id)
            };

            int totalItems = query.Count();


            if (page.HasValue && pageSize.HasValue)
            {
                query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }

            var result = query.ToList();

            var response = _mapper.Map<ICollection<MajorResponse>>(result);

            return result.Any() ? ApiResponse<ICollection<MajorResponse>>
            .Success(response, page, pageSize, totalItems)
            : ApiResponse<ICollection<MajorResponse>>.NotFound("Không có dữ liệu");
        }

        public ApiResponse<MajorResponse> GetMajorById(long id)
        {
            var major = _repository.GetMajorById(id);
            return (major != null && (major.Active == true))
                ? ApiResponse<MajorResponse>.Success(_mapper.Map<MajorResponse>(major))
                : ApiResponse<MajorResponse>.NotFound($"Không tìm thấy chuyên ngành #{id}");
        }

        public ApiResponse<MajorResponse> CreateMajor(MajorRequest majorRequest)
        {
            var existing = _repository.GetMajor().FirstOrDefault(us => us.Name?.ToLower() == majorRequest.Name.ToLower());
            if (existing != null)
            {
                return ApiResponse<MajorResponse>.Conflict("Tên chuyên ngành đã tồn tại");
            }

            var created = _repository.CreateMajor(new Major() { Name = majorRequest.Name, Description = majorRequest.Description, Active = true });
            return ApiResponse<MajorResponse>.Success(_mapper.Map<MajorResponse>(created));
        }

        public ApiResponse<MajorResponse> UpdateMajor(long id, MajorRequest majorRequest)
        {
            var existingMajor = _repository.GetMajorById(id);
            if (existingMajor == null || existingMajor.Active == false)
            {
                return ApiResponse<MajorResponse>.NotFound("Không tìm thấy chuyên ngành.");
            }

            var existing = _repository.GetMajor()
                .FirstOrDefault(us => us.Id != id && us.Name?.ToLower() == majorRequest.Name.ToLower());
            if (existing != null)
            {
                return ApiResponse<MajorResponse>.Conflict("Tên chuyên ngành đã tồn tại");
            }


            existingMajor.Name = majorRequest.Name;
            existingMajor.Description = majorRequest.Description;
            //existingMajor.Active = false;
            _repository.UpdateMajor(existingMajor);
            return ApiResponse<MajorResponse>.Success(_mapper.Map<MajorResponse>(existingMajor));
        }

        public ApiResponse<Major> DeleteMajor(long id)
        {
            var existingMajor = _repository.GetMajorById(id);
            if (existingMajor == null)
            {
                return ApiResponse<Major>.NotFound("Không tìm thấy chuyên ngành.");
            }

            if (existingMajor.Active == false)
            {
                return ApiResponse<Major>.Conflict("chuyên ngành không tồn tại.");
            }

            existingMajor.Active = false;
            _repository.DeleteMajor(existingMajor);

            return ApiResponse<Major>.Success();
        }
    }
}
