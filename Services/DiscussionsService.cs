using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.DTOs.Requests;
using AutoMapper;
using System;
namespace ISC_ELIB_SERVER.Services
{

    public interface IDiscussionsService
    {
        ApiResponse<ICollection<DiscussionResponse>> GetDiscussions(int page, int pageSize, string search, string sortColumn, string sortOrder);
        ApiResponse<DiscussionResponse> GetDiscussionById(long id);
        ApiResponse<DiscussionResponse> CreateDiscussion(DiscussionRequest request);
        ApiResponse<DiscussionResponse> UpdateDiscussion(long id, DiscussionRequest request);
        ApiResponse<bool> DeleteDiscussion(long id);
    }

    public class DiscussionsService : IDiscussionsService
    {
        private readonly DiscussionRepo _repository;
        private readonly IMapper _mapper;

        public DiscussionsService(DiscussionRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public ApiResponse<ICollection<DiscussionResponse>> GetDiscussions(int page, int pageSize, string search, string sortColumn, string sortOrder)
        {
            var query = _repository.GetDiscussions().AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(d => d.Content != null && d.Content.ToLower().Contains(search.ToLower()));
            }

            query = sortColumn switch
            {
                "CreateAt" => sortOrder.ToLower() == "desc" ? query.OrderByDescending(d => d.CreateAt) : query.OrderBy(d => d.CreateAt),
                "Id" => sortOrder.ToLower() == "desc" ? query.OrderByDescending(d => d.Id) : query.OrderBy(d => d.Id),
                _ => query.OrderBy(d => d.Id)
            };

            var result = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            var response = _mapper.Map<ICollection<DiscussionResponse>>(result);

            return result.Any()
                ? ApiResponse<ICollection<DiscussionResponse>>.Success(response)
                : ApiResponse<ICollection<DiscussionResponse>>.NotFound("Không có dữ liệu");
        }

        public ApiResponse<DiscussionResponse> GetDiscussionById(long id)
        {
            var discussion = _repository.GetDiscussionById(id);
            return discussion != null
                ? ApiResponse<DiscussionResponse>.Success(_mapper.Map<DiscussionResponse>(discussion))
                : ApiResponse<DiscussionResponse>.NotFound($"Không tìm thấy thảo luận #{id}");
        }

        public ApiResponse<DiscussionResponse> CreateDiscussion(DiscussionRequest request)
        {
            var discussion = _mapper.Map<Discussion>(request);
            discussion.CreateAt = DateTime.UtcNow;

            var createdDiscussion = _repository.CreateDiscussion(discussion);
            return ApiResponse<DiscussionResponse>.Success(_mapper.Map<DiscussionResponse>(createdDiscussion));
        }

        public ApiResponse<DiscussionResponse> UpdateDiscussion(long id, DiscussionRequest request)
        {
            var existingDiscussion = _repository.GetDiscussionById(id);
            if (existingDiscussion == null)
            {
                return ApiResponse<DiscussionResponse>.NotFound($"Không tìm thấy thảo luận #{id}");
            }

            existingDiscussion.Content = request.Content;
            existingDiscussion.TopicId = request.TopicId;
            existingDiscussion.UserId = request.UserId;

            var updatedDiscussion = _repository.UpdateDiscussion(existingDiscussion);
            return ApiResponse<DiscussionResponse>.Success(_mapper.Map<DiscussionResponse>(updatedDiscussion));
        }

        public ApiResponse<bool> DeleteDiscussion(long id)
        {
            var success = _repository.DeleteDiscussion(id);
            return success
                ? ApiResponse<bool>.Success(true)
                : ApiResponse<bool>.NotFound("Không tìm thấy thảo luận để xóa");
        }
    }


}
