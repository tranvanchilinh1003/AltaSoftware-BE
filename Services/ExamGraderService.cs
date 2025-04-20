using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;

namespace ISC_ELIB_SERVER.Services
{

    public interface IExamGraderService
    {
        ApiResponse<PagedResult<ExamGraderResponse>> GetAll(int page, int pageSize, string? search, string? sortBy, bool isDescending);
        ApiResponse<ExamGraderResponse> GetById(long id);
        ApiResponse<ExamGraderResponse> Create(ExamGraderRequest request);
        ApiResponse<ExamGraderResponse> Update(long id, ExamGraderRequest request);
        ApiResponse<object> Delete(long id);
        
    }
    public class ExamGraderService : IExamGraderService
    {
        private readonly ExamGraderRepo _repository;
        private readonly IMapper _mapper;

        public ExamGraderService(ExamGraderRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public ApiResponse<PagedResult<ExamGraderResponse>> GetAll(int page, int pageSize, string? search, string? sortBy, bool isDescending)
        {
            var entities = _repository.GetAll(page, pageSize, search, sortBy, isDescending);

            var responses = _mapper.Map<ICollection<ExamGraderResponse>>(entities.Items);
            var result = new PagedResult<ExamGraderResponse>(responses, entities.TotalItems, page, pageSize);


            return ApiResponse<PagedResult<ExamGraderResponse>>.Success(result);
        }

        public ApiResponse<ExamGraderResponse> GetById(long id)
        {
            var entity = _repository.GetById(id);
            if (entity == null) return ApiResponse<ExamGraderResponse>.NotFound("ExamGrader không tồn tại");

            var response = _mapper.Map<ExamGraderResponse>(entity);
            return ApiResponse<ExamGraderResponse>.Success(response);
        }

        public ApiResponse<ExamGraderResponse> Create(ExamGraderRequest request)
        {
            var entity = _mapper.Map<ExamGrader>(request);
            _repository.Create(entity);

            var response = _mapper.Map<ExamGraderResponse>(entity);
            return ApiResponse<ExamGraderResponse>.Success(response);
        }

        public ApiResponse<ExamGraderResponse> Update(long id, ExamGraderRequest request)
        {
            var entity = _repository.GetById(id);
            if (entity == null) return ApiResponse<ExamGraderResponse>.NotFound("ExamGrader không tồn tại");

            _mapper.Map(request, entity);
            _repository.Update(entity);

            var response = _mapper.Map<ExamGraderResponse>(entity);
            return ApiResponse<ExamGraderResponse>.Success(response);
        }
        
        public ApiResponse<object> Delete(long id)
        {
            var result = _repository.Delete(id);
            return result
                ? ApiResponse<object>.Success()
                : ApiResponse<object>.NotFound("ExamGrader không tồn tại");
        }
    }
}
