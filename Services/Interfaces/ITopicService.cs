using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;

namespace ISC_ELIB_SERVER.Services
{
    public interface ITopicService
    {
        ApiResponse<ICollection<TopicResponse>> GetTopics(int page, int pageSize, string search, string sortColumn, string sortOrder);
        ApiResponse<TopicResponse> GetTopicById(int id);
        ApiResponse<TopicResponse> CreateTopic(TopicRequest request);
        ApiResponse<TopicResponse> UpdateTopic(int id, TopicRequest request);
        ApiResponse<string> DeleteTopic(int id);
    }

    public class TopicService : ITopicService
    {
        private readonly TopicRepo _topicRepo;
        private readonly CloudinaryService _cloudinaryService;
        private readonly IMapper _mapper;

        public TopicService(TopicRepo topicRepo, CloudinaryService cloudinaryService, IMapper mapper)
        {
            _topicRepo = topicRepo;
            _cloudinaryService = cloudinaryService;
            _mapper = mapper;
        }

        public ApiResponse<ICollection<TopicResponse>> GetTopics(int page, int pageSize, string search, string sortColumn, string sortOrder)
        {
            var query = _topicRepo.GetAllTopics().AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(t => t.Name.ToLower().Contains(search.ToLower()));
            }

            query = sortColumn switch
            {
                "Name" => sortOrder.ToLower() == "desc" ? query.OrderByDescending(t => t.Name) : query.OrderBy(t => t.Name),
                "Id" => sortOrder.ToLower() == "desc" ? query.OrderByDescending(t => t.Id) : query.OrderBy(t => t.Id),
                _ => query.OrderBy(t => t.Id)
            };

            var result = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var response = _mapper.Map<ICollection<TopicResponse>>(result);

            return result.Any()
                ? ApiResponse<ICollection<TopicResponse>>.Success(response)
                : ApiResponse<ICollection<TopicResponse>>.NotFound("Không có dữ liệu");
        }

        public ApiResponse<TopicResponse> GetTopicById(int id)
        {
            var topic = _topicRepo.GetTopicById(id);
            return topic != null
                ? ApiResponse<TopicResponse>.Success(_mapper.Map<TopicResponse>(topic))
                : ApiResponse<TopicResponse>.NotFound($"Không tìm thấy Topic có id {id}");
        }

        public ApiResponse<TopicResponse> CreateTopic(TopicRequest request)
        {
            var existing = _topicRepo.GetAllTopics().FirstOrDefault(t => t.Name?.ToLower() == request.Name.ToLower());
            if (existing != null)
            {
                return ApiResponse<TopicResponse>.Conflict("Tên Topic đã tồn tại");
            }

            var topic = _mapper.Map<Topic>(request);

            // Fix lỗi DateTime bằng cách chuyển đổi EndDate
            topic.EndDate = DateTimeUtils.ConvertToUnspecified(topic.EndDate);
            topic.File = _cloudinaryService.UploadBase64Async(request.File).Result;
            var create = _topicRepo.CreateTopic(topic);
            return ApiResponse<TopicResponse>.Success(_mapper.Map<TopicResponse>(create));
        }


        public ApiResponse<TopicResponse> UpdateTopic(int id, TopicRequest request)
        {
            var topic = _topicRepo.GetTopicById(id);
            if (topic == null)
            {
                return ApiResponse<TopicResponse>.NotFound($"Không tìm thấy Topic có id {id}");
            }

            _mapper.Map(request, topic);

            // Fix lỗi DateTime bằng cách chuyển đổi EndDate
            topic.EndDate = DateTimeUtils.ConvertToUnspecified(topic.EndDate);

            var update = _topicRepo.UpdateTopic(topic);
            return ApiResponse<TopicResponse>.Success(_mapper.Map<TopicResponse>(update));
        }


        public ApiResponse<string> DeleteTopic(int id)
        {
            var delete = _topicRepo.DeleteTopic(id);
            return delete
                ? ApiResponse<string>.Success("Xóa Topic thành công")
                : ApiResponse<string>.NotFound($"Không tìm thấy Topic có id {id}");
        }
    }
}
