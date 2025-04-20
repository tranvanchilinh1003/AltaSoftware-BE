using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;
using ISC_ELIB_SERVER.Services;

namespace ISC_ELIB_SERVER.Services
{
    public class TeacherFamilyService : ITeacherFamilyService
    {
        private readonly TeacherFamilyRepo _repository;
        private readonly IMapper _mapper;

        public TeacherFamilyService(TeacherFamilyRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public ApiResponse<ICollection<TeacherFamilyResponse>> GetTeacherFamilies()
        {
            var teacherFamilies = _repository.GetTeacherFamilies();
            var response = _mapper.Map<ICollection<TeacherFamilyResponse>>(teacherFamilies);
            return ApiResponse<ICollection<TeacherFamilyResponse>>.Success(response);
        }

        public ApiResponse<TeacherFamilyResponse> GetTeacherFamilyById(long id)
        {
            var teacherFamily = _repository.GetTeacherFamilyById(id);
            return teacherFamily != null
                ? ApiResponse<TeacherFamilyResponse>.Success(_mapper.Map<TeacherFamilyResponse>(teacherFamily))
                : ApiResponse<TeacherFamilyResponse>.NotFound("Không tìm thấy thông tin gia đình giáo viên");
        }

        public ApiResponse<TeacherFamilyResponse> CreateTeacherFamily(TeacherFamilyRequest request)
        {
            var teacherFamily = _mapper.Map<TeacherFamily>(request);
            var created = _repository.CreateTeacherFamily(teacherFamily);
            return ApiResponse<TeacherFamilyResponse>.Success(_mapper.Map<TeacherFamilyResponse>(created));
        }

        public ApiResponse<TeacherFamilyResponse> UpdateTeacherFamily(long id, TeacherFamilyRequest request)
        {
            var teacherFamily = _repository.GetTeacherFamilyById(id);
            if (teacherFamily == null) return ApiResponse<TeacherFamilyResponse>.NotFound("Không tìm thấy bản ghi");

            _mapper.Map(request, teacherFamily);
            _repository.UpdateTeacherFamily(teacherFamily);

            return ApiResponse<TeacherFamilyResponse>.Success(_mapper.Map<TeacherFamilyResponse>(teacherFamily));
        }

        public ApiResponse<object> DeleteTeacherFamily(long id)
        {
            return _repository.DeleteTeacherFamily(id)
                ? ApiResponse<object>.Success()
                : ApiResponse<object>.NotFound("Không tìm thấy thông tin gia đình giáo viên để xóa");
        }
    }
}