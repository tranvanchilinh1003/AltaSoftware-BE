using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using AutoMapper;

namespace ISC_ELIB_SERVER.Services
{
    public class AnswerImagesQaService : IAnswerImagesQaService
    {
        private readonly AnswerImagesQaRepo _repository;
        private readonly IMapper _mapper;

        public AnswerImagesQaService(AnswerImagesQaRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public ApiResponse<ICollection<AnswerImagesQaResponse>> GetAnswerImages(long? answerId)
        {
            var query = _repository.GetAnswerImages().AsQueryable();

            if (answerId.HasValue)
            {
                query = query.Where(a => a.AnswerId == answerId.Value);
            }

            var result = query.ToList();
            var response = _mapper.Map<ICollection<AnswerImagesQaResponse>>(result);

            return result.Any()
                ? ApiResponse<ICollection<AnswerImagesQaResponse>>.Success(response)
                : ApiResponse<ICollection<AnswerImagesQaResponse>>.NotFound("Không có dữ liệu");
        }

        public ApiResponse<AnswerImagesQaResponse> GetAnswerImageById(long id)
        {
            var image = _repository.GetAnswerImageById(id);
            return image != null
                ? ApiResponse<AnswerImagesQaResponse>.Success(_mapper.Map<AnswerImagesQaResponse>(image))
                : ApiResponse<AnswerImagesQaResponse>.NotFound($"Không tìm thấy ảnh #{id}");
        }

        public ApiResponse<AnswerImagesQaResponse> CreateAnswerImage(AnswerImagesQaRequest request)
        {
            var created = _repository.CreateAnswerImage(new AnswerImagesQa()
            {
                AnswerId = request.AnswerId,
                ImageUrl = request.ImageUrl
            });

            return ApiResponse<AnswerImagesQaResponse>.Success(_mapper.Map<AnswerImagesQaResponse>(created));
        }

        public ApiResponse<AnswerImagesQaResponse> UpdateAnswerImage(long id, AnswerImagesQaRequest request)
        {
            var existing = _repository.GetAnswerImageById(id);
            if (existing == null)
            {
                return ApiResponse<AnswerImagesQaResponse>.NotFound("Ảnh không tồn tại");
            }

            existing.ImageUrl = request.ImageUrl;
            var updated = _repository.UpdateAnswerImage(existing);

            return ApiResponse<AnswerImagesQaResponse>.Success(_mapper.Map<AnswerImagesQaResponse>(updated));
        }

        public ApiResponse<AnswerImagesQaResponse> DeleteAnswerImage(long id)
        {
            var success = _repository.DeleteAnswerImage(id);
            return success
                ? ApiResponse<AnswerImagesQaResponse>.Success()
                : ApiResponse<AnswerImagesQaResponse>.NotFound("Không tìm thấy ảnh để xóa");
        }
    }
}
