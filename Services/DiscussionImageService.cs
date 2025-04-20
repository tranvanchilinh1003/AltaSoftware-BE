using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;

namespace ISC_ELIB_SERVER.Services
{
    public interface IDiscussionImageService
    {
        ApiResponse<ICollection<DiscussionImageResponse>> GetDiscussionImagesByDiscussionId(long discussionId);
        ApiResponse<DiscussionImageResponse> GetDiscussionImageById(long id);
        ApiResponse<DiscussionImageResponse> CreateDiscussionImage(DiscussionImageRequest request);
        ApiResponse<DiscussionImageResponse> UpdateDiscussionImage(long id, DiscussionImageRequest request);
        ApiResponse<bool> DeleteDiscussionImage(long id);
    }
    public class DiscussionImageService : IDiscussionImageService
    {
        private readonly DiscussionImageRepo _repository;
        private readonly CloudinaryService _cloudinaryService;
        private readonly IMapper _mapper;

        public DiscussionImageService(DiscussionImageRepo repository, CloudinaryService cloudinaryService, IMapper mapper)
        {
            _repository = repository;
            _cloudinaryService = cloudinaryService;
            _mapper = mapper;
        }

        // Lấy danh sách ảnh theo discussionId
        public ApiResponse<ICollection<DiscussionImageResponse>> GetDiscussionImagesByDiscussionId(long discussionId)
        {
            var images = _repository.GetDiscussionImagesByDiscussionId(discussionId);
            var response = _mapper.Map<ICollection<DiscussionImageResponse>>(images);

            return images.Any()
                ? ApiResponse<ICollection<DiscussionImageResponse>>.Success(response)
                : ApiResponse<ICollection<DiscussionImageResponse>>.NotFound("Không có ảnh nào cho cuộc thảo luận này");
        }

        // Lấy ảnh theo Id
        public ApiResponse<DiscussionImageResponse> GetDiscussionImageById(long id)
        {
            var image = _repository.GetDiscussionImageById(id);
            return image != null
                ? ApiResponse<DiscussionImageResponse>.Success(_mapper.Map<DiscussionImageResponse>(image))
                : ApiResponse<DiscussionImageResponse>.NotFound($"Không tìm thấy ảnh #{id}");
        }

        // Thêm ảnh mới
        public ApiResponse<DiscussionImageResponse> CreateDiscussionImage(DiscussionImageRequest request)
        {
            var newImage = _repository.CreateDiscussionImage(new DiscussionImage
            {
                DiscussionId = request.DiscussionId,
                ImageUrl = _cloudinaryService.UploadBase64Async(request.ImageUrl).Result,
            });

            return ApiResponse<DiscussionImageResponse>.Success(_mapper.Map<DiscussionImageResponse>(newImage));
        }

        // Cập nhật hình ảnh thảo luận
        public ApiResponse<DiscussionImageResponse> UpdateDiscussionImage(long id, DiscussionImageRequest request)
        {
            var discussionImage = _repository.GetDiscussionImageById(id);
            if (discussionImage == null)
            {
                return ApiResponse<DiscussionImageResponse>.NotFound($"Không tìm thấy hình ảnh #{id}");
            }

            discussionImage.ImageUrl = request.ImageUrl;

            var updatedImage = _repository.UpdateDiscussionImage(discussionImage);
            return ApiResponse<DiscussionImageResponse>.Success(_mapper.Map<DiscussionImageResponse>(updatedImage));
        }

        // Xóa ảnh theo ID
        public ApiResponse<bool> DeleteDiscussionImage(long id)
        {
            var success = _repository.DeleteDiscussionImage(id);
            return success
                ? ApiResponse<bool>.Success(true)
                : ApiResponse<bool>.NotFound("Không tìm thấy ảnh để xóa");
        }
    }
}