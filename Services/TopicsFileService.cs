using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;

namespace ISC_ELIB_SERVER.Services
{
    public class TopicsFileService : ITopicsFileService
    {
        private readonly ITopicsFileRepo _topicsFileRepo;
        private readonly IMapper _mapper;
        private readonly isc_dbContext _context;

        public TopicsFileService(ITopicsFileRepo topicsFileRepo, IMapper mapper, isc_dbContext context)
        {
            _topicsFileRepo = topicsFileRepo;
            _mapper = mapper;
            _context = context;

        }

        public ApiResponse<ICollection<TopicsFileResponse>> GetAll(int page, int pageSize, string? search, string sortColumn, string sortOrder)
        {
            var query = _topicsFileRepo.GetAll().AsQueryable();
            // Tìm kiếm theo tên file (hoặc các thuộc tính khác của TopicsFile)
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(t => t.FileName.ToLower().Contains(search.ToLower()));
            }
            //if (!string.IsNullOrEmpty(search))
            // Sắp xếp theo cột và thứ tự (ascending/descending)
            query = sortColumn switch
            {
                "FileName" => sortOrder.ToLower() == "desc" ? query.OrderByDescending(t => t.FileName) : query.OrderBy(t => t.FileName),
                "Id" => sortOrder.ToLower() == "desc" ? query.OrderByDescending(t => t.Id) : query.OrderBy(t => t.Id),
                _ => query.OrderBy(t => t.Id)
            };
            // Áp dụng phân trang
            var result = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            // Map các đối tượng thành đối tượng DTO
            var response = _mapper.Map<ICollection<TopicsFileResponse>>(result);
            return result.Any()
                ? ApiResponse<ICollection<TopicsFileResponse>>.Success(response)
                : ApiResponse<ICollection<TopicsFileResponse>>.NotFound("Không có dữ liệu");
        }


        public ApiResponse<TopicsFileResponse> GetById(int id)
        {
            var topicsFile = _topicsFileRepo.GetById(id);
            return topicsFile != null
                ? ApiResponse<TopicsFileResponse>.Success(_mapper.Map<TopicsFileResponse>(topicsFile))
                : ApiResponse<TopicsFileResponse>.NotFound($"Không tìm thấy TopicsFile có id {id}");
        }

        public ApiResponse<TopicsFileResponse> Create(TopicsFileRequest request)
        {
            // Kiểm tra topic_id có tồn tại trong bảng topics không
            var topicExists = _context.Topics.Any(t => t.Id == request.TopicId);
            if (!topicExists)
            {
                return ApiResponse<TopicsFileResponse>.BadRequest($"Không tìm thấy Topic có id {request.TopicId}");
            }
            try
            {
                var topicsFile = _mapper.Map<TopicsFile>(request);
                // Kiểm tra xem TopicId có được map đúng không
                Console.WriteLine($"TopicId của topicsFile: {topicsFile.TopicId}");
                // Đảm bảo TopicId được gán đúng
                topicsFile.TopicId = request.TopicId;
                // Lưu thời gian theo UTC
                topicsFile.CreateAt = DateTime.UtcNow;
                topicsFile.CreateAt = DateTimeUtils.ConvertToUnspecified(topicsFile.CreateAt);

                var create = _topicsFileRepo.Create(topicsFile);
                return ApiResponse<TopicsFileResponse>.Success(_mapper.Map<TopicsFileResponse>(create));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi khi thêm dữ liệu: {ex.Message}");
                return ApiResponse<TopicsFileResponse>.BadRequest("Lỗi khi thêm dữ liệu");
            }
        }


        public ApiResponse<TopicsFileResponse> Update(int id, TopicsFileRequest request)
        {
            var topicExists = _context.Topics.Any(t => t.Id == request.TopicId);
            if (!topicExists)
            {
                return ApiResponse<TopicsFileResponse>.BadRequest($"Không tìm thấy Topic có id {request.TopicId}");
            }
            var topicsFile = _topicsFileRepo.GetById(id);
            if (topicsFile == null)
            {
                return ApiResponse<TopicsFileResponse>.NotFound($"Không tìm thấy TopicsFile có id {id}");
            }

            topicsFile.CreateAt = DateTime.UtcNow;
            topicsFile.CreateAt = DateTimeUtils.ConvertToUnspecified(topicsFile.CreateAt);

            _mapper.Map(request, topicsFile);
            var update = _topicsFileRepo.Update(topicsFile);
            return ApiResponse<TopicsFileResponse>.Success(_mapper.Map<TopicsFileResponse>(update));
        }

        public ApiResponse<string> Delete(int id)
        {
            var delete = _topicsFileRepo.Delete(id);
            return delete
                ? ApiResponse<string>.Success("Xóa TopicsFile thành công")
                : ApiResponse<string>.NotFound($"Không tìm thấy TopicsFile có id {id}");
        }
    }
}
