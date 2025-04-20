using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using AutoMapper;

namespace ISC_ELIB_SERVER.Services
{
    public class QuestionImagesQaService : IQuestionImagesQaService
    {
        private readonly QuestionImagesQaRepo _repository;
        private readonly IMapper _mapper;

        public QuestionImagesQaService(QuestionImagesQaRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public ApiResponse<ICollection<QuestionImagesQaResponse>> GetQuestionImages(long? questionId)
        {
            var query = _repository.GetQuestionImages().AsQueryable();

            if (questionId.HasValue)
            {
                query = query.Where(q => q.QuestionId == questionId.Value);
            }

            var result = query.ToList();
            var response = _mapper.Map<ICollection<QuestionImagesQaResponse>>(result);

            return result.Any()
                ? ApiResponse<ICollection<QuestionImagesQaResponse>>.Success(response)
                : ApiResponse<ICollection<QuestionImagesQaResponse>>.NotFound("Không có dữ liệu");
        }

        public ApiResponse<QuestionImagesQaResponse> GetQuestionImageById(long id)
        {
            var image = _repository.GetQuestionImageById(id);
            return image != null
                ? ApiResponse<QuestionImagesQaResponse>.Success(_mapper.Map<QuestionImagesQaResponse>(image))
                : ApiResponse<QuestionImagesQaResponse>.NotFound($"Không tìm thấy ảnh #{id}");
        }

        public ApiResponse<QuestionImagesQaResponse> CreateQuestionImage(QuestionImagesQaRequest request)
        {
            var created = _repository.CreateQuestionImage(new QuestionImagesQa()
            {
                QuestionId = request.QuestionId,
                ImageUrl = request.ImageUrl
            });

            return ApiResponse<QuestionImagesQaResponse>.Success(_mapper.Map<QuestionImagesQaResponse>(created));
        }

        public ApiResponse<QuestionImagesQaResponse> UpdateQuestionImage(long id, QuestionImagesQaRequest request)
        {
            var existing = _repository.GetQuestionImageById(id);
            if (existing == null)
            {
                return ApiResponse<QuestionImagesQaResponse>.NotFound("Ảnh không tồn tại");
            }

            existing.ImageUrl = request.ImageUrl;
            var updated = _repository.UpdateQuestionImage(existing);

            return ApiResponse<QuestionImagesQaResponse>.Success(_mapper.Map<QuestionImagesQaResponse>(updated));
        }

        public ApiResponse<QuestionImagesQaResponse> DeleteQuestionImage(long id)
        {
            var success = _repository.DeleteQuestionImage(id);
            return success
                ? ApiResponse<QuestionImagesQaResponse>.Success()
                : ApiResponse<QuestionImagesQaResponse>.NotFound("Không tìm thấy ảnh để xóa");
        }
    }
}
