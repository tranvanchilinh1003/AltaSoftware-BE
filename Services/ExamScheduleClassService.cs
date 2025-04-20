using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;

namespace ISC_ELIB_SERVER.Services
{
    public interface IExamScheduleClassService
    {
        ApiResponse<PagedResult<ExamScheduleClassResponse>> GetAll(int page, int pageSize, string? searchTerm, string? sortBy, string? sortOrder);
        ApiResponse<ExamScheduleClassResponse> GetById(long id);
        ApiResponse<ExamScheduleClassResponse> Create(ExamScheduleClassRequest request);
        ApiResponse<ExamScheduleClassResponse> Update(long id, ExamScheduleClassRequest request);
        ApiResponse<string> UpdateStudentCount(int examScheduleId, int classId, int studentCount);
        ApiResponse<string> RemoveClassFromSchedule(int examScheduleId, int classId);

    }
    public class ExamScheduleClassService: IExamScheduleClassService
    {
        private readonly ExamScheduleClassRepo _repository;
        private readonly IMapper _mapper;

        public ExamScheduleClassService(ExamScheduleClassRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public ApiResponse<PagedResult<ExamScheduleClassResponse>> GetAll(int page, int pageSize, string? searchTerm, string? sortBy, string? sortOrder)
        {
            var entities = _repository.GetAll(page, pageSize, searchTerm, sortBy, sortOrder);
            var responses = _mapper.Map<ICollection<ExamScheduleClassResponse>>(entities.Items);
            var result = new PagedResult<ExamScheduleClassResponse>(responses, entities.TotalItems, page, pageSize);

            return ApiResponse<PagedResult<ExamScheduleClassResponse>>.Success(result);
        }


        public ApiResponse<ExamScheduleClassResponse> GetById(long id)
        {
            var entity = _repository.GetById(id);
            if (entity == null) return ApiResponse<ExamScheduleClassResponse>.NotFound("ExamScheduleClass không tồn tại");

            var response = _mapper.Map<ExamScheduleClassResponse>(entity);
            return ApiResponse<ExamScheduleClassResponse>.Success(response);
        }

        public ApiResponse<ExamScheduleClassResponse> Create(ExamScheduleClassRequest request)
        {
            var entity = _mapper.Map<ExamScheduleClass>(request);
            _repository.Create(entity);

            var response = _mapper.Map<ExamScheduleClassResponse>(entity);
            return ApiResponse<ExamScheduleClassResponse>.Success(response);
        }

        public ApiResponse<ExamScheduleClassResponse> Update(long id, ExamScheduleClassRequest request)
        {
            var entity = _repository.GetById(id);
            if (entity == null) return ApiResponse<ExamScheduleClassResponse>.NotFound("ExamScheduleClass không tồn tại");

            _mapper.Map(request, entity);
            _repository.Update(entity);

            var response = _mapper.Map<ExamScheduleClassResponse>(entity);
            return ApiResponse<ExamScheduleClassResponse>.Success(response);
        }
        public ApiResponse<string> UpdateStudentCount(int examScheduleId, int classId, int studentCount)
        {
            try
            {
                _repository.UpdateStudentCount(examScheduleId, classId, studentCount);
                return ApiResponse<string>.Success("Cập nhật thành công");
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.Error(new Dictionary<string, string[]>
        {
            { "Exception", new[] { ex.Message } }
        });
            }
        }

        public ApiResponse<string> RemoveClassFromSchedule(int examScheduleId, int classId)
        {
            try
            {
                _repository.RemoveClassFromSchedule(examScheduleId, classId);
                return ApiResponse<string>.Success("Xóa lớp khỏi lịch thi thành công");
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.Error(new Dictionary<string, string[]>
        {
            { "Exception", new[] { ex.Message } }
        });
            }
        }
    }
}
