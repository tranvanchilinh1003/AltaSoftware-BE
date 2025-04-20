using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.DTOs.Requests;
using AutoMapper;

namespace ISC_ELIB_SERVER.Services
{
    public class CampusService : ICampusService
    {
        private readonly CampusRepo _repository;
        private readonly IMapper _mapper;
        private readonly SchoolRepo _schoolRepository;
        private readonly UserRepo _userRepository;

        public CampusService(CampusRepo repository, IMapper mapper, SchoolRepo schoolRepository, UserRepo userRepository)
        {
            _repository = repository;
            _mapper = mapper;
            _schoolRepository = schoolRepository;
            _userRepository = userRepository;
        }

        public ApiResponse<ICollection<CampusResponse>> GetCampuses(int? page, int? pageSize, string? search, string? sortColumn, string? sortOrder)
        {
            var query = _repository.GetCampuses().AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(c => c.Name.Contains(search));
            }

            query = sortColumn switch
            {
                "Id" => sortOrder.ToLower() == "desc" ? query.OrderByDescending(us => us.Id) : query.OrderBy(us => us.Id),
                _ => query.OrderBy(c => c.Id)
            };

            if (page.HasValue && pageSize.HasValue)
            {
                query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }

            var result = query.ToList();
            var response = _mapper.Map<ICollection<CampusResponse>>(result);

            return result.Any() ? ApiResponse<ICollection<CampusResponse>>
            .Success(response, page, pageSize, _repository.GetCampuses().Count) :
            ApiResponse<ICollection<CampusResponse>>.NotFound("Không có dữ liệu");
        }

        public ApiResponse<CampusResponse> GetCampusById(long id)
        {
            var campus = _repository.GetCampusById(id);
            return campus != null
                ? ApiResponse<CampusResponse>.Success(_mapper.Map<CampusResponse>(campus))
                : ApiResponse<CampusResponse>.NotFound($"Không tìm thấy cơ sở #{id}");
        }

        public ApiResponse<CampusResponse> CreateCampus(CampusRequest campusRequest)
        {
            if (_schoolRepository.GetSchoolById((long)campusRequest.SchoolId) == null)
            {
                return ApiResponse<CampusResponse>.BadRequest("Mã trường không tồn tại");
            }

            if (_userRepository.GetUserById(campusRequest.UserId ?? 0) == null)
            {
                return ApiResponse<CampusResponse>.BadRequest("Mã người dùng không tồn tại");
            }

            var newCampus = new Campus
            {
                Name = campusRequest.Name,
                Address = campusRequest.Address,
                PhoneNumber = campusRequest.PhoneNumber,
                SchoolId = campusRequest.SchoolId,
                UserId = campusRequest.UserId
            };

            try
            {
                var created = _repository.CreateCampus(newCampus);
                return ApiResponse<CampusResponse>.Success(_mapper.Map<CampusResponse>(created));
            }
            catch (Exception)
            {
                return ApiResponse<CampusResponse>.BadRequest("Lỗi khi tạo cơ sở");
            }
        }

        public ApiResponse<CampusResponse> UpdateCampus(long id, CampusRequest campusRequest)
        {
            var existing = _repository.GetCampusById(id);
            if (existing == null)
            {
                return ApiResponse<CampusResponse>.NotFound($"Không tìm thấy cơ sở #{id}");
            }

            if (_schoolRepository.GetSchoolById((long)campusRequest.SchoolId) == null)
            {
                return ApiResponse<CampusResponse>.BadRequest("Mã trường không tồn tại");
            }

            if (_userRepository.GetUserById(campusRequest.UserId ?? 0) == null)
            {
                return ApiResponse<CampusResponse>.BadRequest("Mã người dùng không tồn tại");
            }

            existing.Name = campusRequest.Name;
            existing.Address = campusRequest.Address;
            existing.PhoneNumber = campusRequest.PhoneNumber;
            existing.SchoolId = campusRequest.SchoolId;
            existing.UserId = campusRequest.UserId;

            try
            {
                var updated = _repository.UpdateCampus(existing);
                return ApiResponse<CampusResponse>.Success(_mapper.Map<CampusResponse>(updated));
            }
            catch (Exception)
            {
                return ApiResponse<CampusResponse>.BadRequest("Lỗi khi cập nhật cơ sở");
            }
        }

        public ApiResponse<bool> DeleteCampus(long id)
        {
            var success = _repository.DeleteCampus(id);
            return success ? ApiResponse<bool>.Success() : ApiResponse<bool>.NotFound($"Không tìm thấy cơ sở #{id} để xóa");
        }
    }
}
