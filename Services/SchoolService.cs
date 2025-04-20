using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.DTOs.Requests;
using AutoMapper;
using ISC_ELIB_SERVER.Utils;
using Newtonsoft.Json;

namespace ISC_ELIB_SERVER.Services
{
    public class SchoolService : ISchoolService
    {
        private readonly SchoolRepo _repository;
        private readonly IMapper _mapper;
        private readonly UserRepo _userRepository;
        private readonly EducationLevelRepo _educationLevelRepository;

        private readonly GhnService _ghnService;

        public SchoolService(SchoolRepo repository, IMapper mapper, UserRepo userRepository,
         EducationLevelRepo educationLevelRepository, GhnService ghnService)
        {
            _repository = repository;
            _mapper = mapper;
            _userRepository = userRepository;
            _educationLevelRepository = educationLevelRepository;
            _ghnService = ghnService;
        }

        public async Task<ApiResponse<ICollection<SchoolResponse>>> GetSchools(int? page, int? pageSize, string? search, string? sortColumn, string? sortOrder)
        {
            var query = _repository.GetSchools().AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(s => s.Name.Contains(search));
            }

            query = sortColumn switch
            {
                "Id" => sortOrder.ToLower() == "desc" ? query.OrderByDescending(us => us.Id) : query.OrderBy(us => us.Id),
                _ => query.OrderBy(s => s.Id)
            };

            if (page.HasValue && pageSize.HasValue)
            {
                query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }

            var result = query.ToList();
            var responses = new List<SchoolResponse>();

            foreach (var school in result)
            {
                var (provinceName, districtName, wardName) = await _ghnService.GetLocationName(school.ProvinceId ?? 0, school.DistrictId ?? 0, school.WardId?.ToString() ?? "");
                var response = _mapper.Map<SchoolResponse>(school);
                response.ProvinceName = provinceName;
                response.DistrictName = districtName;
                response.WardName = wardName;
                responses.Add(response);
            }

            return responses.Any() ? ApiResponse<ICollection<SchoolResponse>>
                .Success(responses, page, pageSize, _repository.GetSchools().Count)
                : ApiResponse<ICollection<SchoolResponse>>.NotFound("Không có dữ liệu");
        }

        public async Task<ApiResponse<SchoolResponse>> GetSchoolById(long id)
        {
            var school = _repository.GetSchoolById(id);
            if (school == null)
                return ApiResponse<SchoolResponse>.NotFound($"Không tìm thấy trường #{id}");

            var (provinceName, districtName, wardName) = await _ghnService.GetLocationName(school.ProvinceId ?? 0, school.DistrictId ?? 0, school.WardId?.ToString() ?? "");
            var response = _mapper.Map<SchoolResponse>(school);
            response.ProvinceName = provinceName;
            response.DistrictName = districtName;
            response.WardName = wardName;
            return ApiResponse<SchoolResponse>.Success(response);
        }

        public ApiResponse<SchoolResponse> CreateSchool(SchoolRequest schoolRequest)
        {
            //if (_userRepository.GetUserById(schoolRequest.UserId ?? 0) == null)
            //{
            //    return ApiResponse<SchoolResponse>.BadRequest("Mã người dùng không tồn tại");
            //}

            if (_educationLevelRepository.GetEducationLevelById((long)schoolRequest.EducationLevelId) == null)
            {
                return ApiResponse<SchoolResponse>.BadRequest("Mã cấp giáo dục không tồn tại");
            }

            string trainingModel = schoolRequest.TrainingModel.ToLower().Replace(" ", "");
            if (!Enum.IsDefined(typeof(TrainingModel), trainingModel))
            {
                return ApiResponse<SchoolResponse>.BadRequest("Mô hình đào tạo không hợp lệ, chỉ chấp nhận 'Công lập' hoặc 'Dân lập'");
            }

            if (_repository.IsSchoolNameExists(schoolRequest.Name))
            {
                return ApiResponse<SchoolResponse>.BadRequest("Tên trường đã tồn tại");
            }

            var newSchool = new School
            {
                Code = schoolRequest.Code,
                Name = schoolRequest.Name,
                ProvinceId = schoolRequest.ProvinceId,
                DistrictId = schoolRequest.DistrictId,
                WardId = schoolRequest.WardId,
                HeadOffice = schoolRequest.HeadOffice,
                SchoolType = schoolRequest.SchoolType,
                PhoneNumber = schoolRequest.PhoneNumber,
                Email = schoolRequest.Email,
                EstablishedDate = DateTime.SpecifyKind(schoolRequest.EstablishedDate, DateTimeKind.Unspecified),
                TrainingModel = EnumUtil.GetEnumStringValue<TrainingModel>(trainingModel),
                WebsiteUrl = schoolRequest.WebsiteUrl,
                UserId = schoolRequest.UserId,
                EducationLevelId = schoolRequest.EducationLevelId
            };

            try
            {
                var created = _repository.CreateSchool(newSchool);
                return ApiResponse<SchoolResponse>.Success(_mapper.Map<SchoolResponse>(created));
            }
            catch (Exception)
            {
                return ApiResponse<SchoolResponse>.BadRequest("Dữ liệu đầu vào không hợp lệ");
            }
        }

        public ApiResponse<SchoolResponse> UpdateSchool(long id, SchoolRequest schoolRequest)
        {
            var existing = _repository.GetSchoolById(id);
            if (existing == null)
            {
                return ApiResponse<SchoolResponse>.NotFound($"Không tìm thấy trường #{id}");
            }

            if (_userRepository.GetUserById(schoolRequest.UserId ?? 0) == null)
            {
                return ApiResponse<SchoolResponse>.BadRequest("Mã người dùng không tồn tại");
            }

            if (_educationLevelRepository.GetEducationLevelById((long)schoolRequest.EducationLevelId) == null)
            {
                return ApiResponse<SchoolResponse>.BadRequest("Mã cấp giáo dục không tồn tại");
            }

            string trainingModel = schoolRequest.TrainingModel.ToLower().Replace(" ", "");
            if (!Enum.IsDefined(typeof(TrainingModel), trainingModel))
            {
                return ApiResponse<SchoolResponse>.BadRequest("Mô hình đào tạo không hợp lệ, chỉ chấp nhận 'Công lập' hoặc 'Dân lập'");
            }

            existing.Code = schoolRequest.Code;
            existing.Name = schoolRequest.Name;
            existing.ProvinceId = schoolRequest.ProvinceId;
            existing.DistrictId = schoolRequest.DistrictId;
            existing.WardId = schoolRequest.WardId;
            existing.HeadOffice = schoolRequest.HeadOffice;
            existing.SchoolType = schoolRequest.SchoolType;
            existing.PhoneNumber = schoolRequest.PhoneNumber;
            existing.Email = schoolRequest.Email;
            existing.EstablishedDate = DateTime.SpecifyKind(schoolRequest.EstablishedDate, DateTimeKind.Unspecified);
            existing.TrainingModel = EnumUtil.GetEnumStringValue<TrainingModel>(trainingModel);
            existing.WebsiteUrl = schoolRequest.WebsiteUrl;
            existing.UserId = schoolRequest.UserId;
            existing.EducationLevelId = schoolRequest.EducationLevelId;

            try
            {
                var updated = _repository.UpdateSchool(existing);
                return ApiResponse<SchoolResponse>.Success(_mapper.Map<SchoolResponse>(updated));
            }
            catch (Exception)
            {
                return ApiResponse<SchoolResponse>.BadRequest("Dữ liệu đầu vào không hợp lệ");
            }
        }
        public ApiResponse<object> DeleteSchool(long id)
        {
            var success = _repository.DeleteSchool(id);
            return success ? ApiResponse<object>.Success() : ApiResponse<object>.NotFound($"Không tìm thấy trường #{id} để xóa");
        }
    }
}
