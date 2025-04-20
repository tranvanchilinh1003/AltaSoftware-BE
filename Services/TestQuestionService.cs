using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;
using ISC_ELIB_SERVER.Services.Interfaces;

namespace ISC_ELIB_SERVER.Services
{


    public class TestQuestionService : ITestQuestionService
    {
        private readonly TestQuestionRepo _repository;
        private readonly IMapper _mapper;

        public TestQuestionService(TestQuestionRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public ApiResponse<ICollection<TestQuestionResponse>> GetTestQuestiones(int page, int pageSize, string search, string sortColumn, string sortOrder)
        {
            var query = _repository.GetTestQuestions().AsQueryable();

            query = sortColumn switch
            {
                "Id" => sortOrder.ToLower() == "desc" ? query.OrderByDescending(ts => ts.Id) : query.OrderBy(ts => ts.Id),
                _ => query.OrderBy(us => us.Id)
            };

            var totalItems = query.Count();
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var result = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var response = _mapper.Map<ICollection<TestQuestionResponse>>(result);

            return result.Any()
                ? ApiResponse<ICollection<TestQuestionResponse>>.Success(response, page, pageSize, totalItems)
                : ApiResponse<ICollection<TestQuestionResponse>>.NotFound("Không có dữ liệu");
        }


        public ApiResponse<TestQuestionResponse> GetTestQuestionById(long id)
        {
            var TestQuestion = _repository.GetTestQuestionById(id);
            return TestQuestion != null
                ? ApiResponse<TestQuestionResponse>.Success(_mapper.Map<TestQuestionResponse>(TestQuestion))
                : ApiResponse<TestQuestionResponse>.NotFound($"Không tìm thấy thông tin #{id}");
        }

        //public ApiResponse<TestQuestionResponse> GetTestQuestionByName(string name)
        //{
        //    var TestQuestion = _repository.GetTestQuestions().FirstOrDefault(ts => ts.Name?.ToLower() == name.ToLower());
        //    return TestQuestion != null
        //        ? ApiResponse<TestQuestionResponse>.Success(_mapper.Map<TestQuestionResponse>(TestQuestion))
        //        : ApiResponse<TestQuestionResponse>.NotFound($"Không tìm thấy bài kiểm tra có tên: {name}");
        //}

        public ApiResponse<TestQuestionResponse> CreateTestQuestion(TestQuestionRequest TestQuestionRequest)
        {
            //var existing = _repository.GetTestQuestions().FirstOrDefault(ts => ts.Name?.ToLower() == TestQuestionRequest.Name.ToLower());
            //if (existing != null)
            //{
            //    return ApiResponse<TestQuestion>.Conflict("Tên bài kiểm tra đã tồn tại");
            //}

            var TestQuestionEntity = _mapper.Map<TestQuestion>(TestQuestionRequest);

            // Tạo mới bài kiểm tra
            var created = _repository.CreateTestQuestion(TestQuestionEntity);

            // Trả về kết quả với kiểu TestQuestionResponse
            return ApiResponse<TestQuestionResponse>.Success(_mapper.Map<TestQuestionResponse>(created));
        }

        public ApiResponse<TestQuestionResponse> UpdateTestQuestion(long id, TestQuestionRequest TestQuestionRequest)
        {

            // Tìm bản ghi cần cập nhật trong database
            var existingTestQuestion = _repository.GetTestQuestionById(id);
            if (existingTestQuestion == null)
            {
                return ApiResponse<TestQuestionResponse>.NotFound($"Không tìm thấy thông tin với Id = {id}");
            }

            // Ánh xạ dữ liệu từ request sang entity, chỉ cập nhật các trường cần thiết
            _mapper.Map(TestQuestionRequest, existingTestQuestion);

            // Thực hiện cập nhật bản ghi
            var updated = _repository.UpdateTestQuestion(existingTestQuestion);
            return ApiResponse<TestQuestionResponse>.Success(_mapper.Map<TestQuestionResponse>(updated));
        }

        public ApiResponse<TestQuestion> DeleteTestQuestion(long id)
        {
            var success = _repository.DeleteTestQuestion(id);
            return success
                ? ApiResponse<TestQuestion>.Success()
                : ApiResponse<TestQuestion>.NotFound("Không tìm thấy");
        }
        
    }
}
