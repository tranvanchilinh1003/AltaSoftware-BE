using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.DTOs.Requests;
using AutoMapper;
using ISC_ELIB_SERVER.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ISC_ELIB_SERVER.Services
{
    public class TestSubmissionAnswerService : ITestSubmissionAnswerService
    {
        private readonly TestSubmissionAnswerRepo _repository;
        private readonly TestAnswerRepo _testAnswerRepo; // Repository kiểm tra đáp án đúng
        private readonly TestQuestionRepo _testQuestionRepo;
        private readonly IMapper _mapper;

        public TestSubmissionAnswerService(
            TestSubmissionAnswerRepo repository,
            TestAnswerRepo testAnswerRepo,
            TestQuestionRepo testQuestionRepo,
            IMapper mapper)
        {
            _repository = repository;
            _testAnswerRepo = testAnswerRepo;
            _testQuestionRepo = testQuestionRepo;
            _mapper = mapper;
        }

        public ApiResponse<ICollection<TestSubmissionAnswerResponse>> GetTestSubmissionAnswers(int page, int pageSize, string search, string sortColumn, string sortOrder)
        {
            var query = _repository.GetAllTestSubmissionAnswers().AsQueryable();

            // Áp dụng sắp xếp theo cột và thứ tự
            query = sortColumn switch
            {
                "Id" => sortOrder.ToLower() == "desc" ? query.OrderByDescending(ts => ts.Id) : query.OrderBy(ts => ts.Id),
                _ => query.OrderBy(ts => ts.Id)
            };

            var totalItems = query.Count();
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var result = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            // Lấy danh sách attachments liên quan đến mỗi TestSubmissionAnswer
            var answerIds = result.Select(ts => ts.Id).ToList();
            var attachments = _repository.GetAttachmentsBySubmissionAnswerIds(answerIds); 

            // Ánh xạ dữ liệu từ TestSubmissionAnswer và Attachments
            var response = result.Select(testSubmissionAnswer =>
            {
                var attachmentsForAnswer = attachments
                    .Where(att => att.TestSubmissionAnswerId == testSubmissionAnswer.Id)
                    .ToList();

                var responseItem = _mapper.Map<TestSubmissionAnswerResponse>(testSubmissionAnswer);
                responseItem.Attachments = _mapper.Map<List<TestSubmissionAnswerAttachmentResponse>>(attachmentsForAnswer); // Giả sử bạn có ánh xạ attachment nếu cần

                return responseItem;
            }).ToList();

            return result.Any()
                 ? ApiResponse<ICollection<TestSubmissionAnswerResponse>>.Success(response, page, pageSize, totalItems)
                 : ApiResponse<ICollection<TestSubmissionAnswerResponse>>.NotFound("Không có dữ liệu");
        }


        public ApiResponse<TestSubmissionAnswerResponse> GetTestSubmissionAnswerById(int id)
        {
            var testSubmissionAnswer = _repository.GetTestSubmissionAnswerById(id);

            if (testSubmissionAnswer != null)
            {
                // Lấy danh sách attachments liên quan đến TestSubmissionAnswer này
                var attachments = _repository.GetAttachmentsBySubmissionAnswerIds(new List<int> { id });

                // Ánh xạ dữ liệu từ TestSubmissionAnswer và Attachments
                var response = _mapper.Map<TestSubmissionAnswerResponse>(testSubmissionAnswer);

                // Ánh xạ attachments vào response
                response.Attachments = _mapper.Map<List<TestSubmissionAnswerAttachmentResponse>>(attachments);

                return ApiResponse<TestSubmissionAnswerResponse>.Success(response);
            }
            else
            {
                return ApiResponse<TestSubmissionAnswerResponse>.NotFound("Không tìm thấy câu trả lời.");
            }
        }


        public async Task<ApiResponse<TestSubmissionAnswerResponse>> CreateTestSubmissionAnswer(TestSubmissionAnswerRequest request, List<IFormFile> attachments)
        {
            if (request == null)
            {
                return new ApiResponse<TestSubmissionAnswerResponse>(
                    1, "Dữ liệu đầu vào không hợp lệ.", null, null);
            }

            try
            {
                // Kiểm tra loại câu hỏi
                bool isMultipleChoice = _testQuestionRepo.IsMultipleChoiceQuestion(request.QuestionId);
                bool isCorrect = false;
                string answerText = null;

                if (isMultipleChoice && request.SelectedAnswerId.HasValue)
                {
                    // Kiểm tra xem câu trả lời đã chọn có đúng không
                    isCorrect = _testAnswerRepo.IsCorrectAnswer(request.SelectedAnswerId.Value);

                    // Lấy AnswerText từ bảng TestAnswer
                    var selectedAnswer = _testAnswerRepo.GetAnswerById(request.SelectedAnswerId.Value);
                    answerText = selectedAnswer?.AnswerText;  // Nếu tìm thấy câu trả lời thì lấy AnswerText
                }

                var testSubmissionAnswer = _mapper.Map<TestSubmissionsAnswer>(request);
                testSubmissionAnswer.IsCorrect = isCorrect;
                testSubmissionAnswer.AnswerText = answerText;  // Gán AnswerText vào bản ghi trả lời

                // Tạo bản ghi câu trả lời
                var createdAnswer = _repository.CreateTestSubmissionAnswer(testSubmissionAnswer);

                // Xử lý file upload từ client
                if (attachments != null && attachments.Any())
                {
                    var fileAttachments = new List<TestSubmissionAnswerAttachment>();

                    foreach (var file in attachments)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await file.CopyToAsync(memoryStream);
                            var base64 = Convert.ToBase64String(memoryStream.ToArray());

                            var newFileName = $"{DateTime.UtcNow:yyyyMMddHHmmssfff}";

                            fileAttachments.Add(new TestSubmissionAnswerAttachment
                            {
                                TestSubmissionAnswerId = createdAnswer.Id,
                                Filename = newFileName,
                                FileBase64 = base64,
                                CreatedAt = DateTime.UtcNow
                            });
                        }
                    }

                    _repository.AddAttachments(fileAttachments);
                }

                _repository.SaveChanges();

                var response = _mapper.Map<TestSubmissionAnswerResponse>(createdAnswer);
                return ApiResponse<TestSubmissionAnswerResponse>.Success(response);
            }
            catch (Exception ex)
            {
                return new ApiResponse<TestSubmissionAnswerResponse>(
                1,
                "Lỗi hệ thống",
                null,
                new Dictionary<string, string[]>
                {
                    { "Exception", new[] { "Không hợp lệ, vui lòng thử lại" } }
                });
            }
        }


        public async Task<ApiResponse<TestSubmissionAnswerResponse>> UpdateTestSubmissionAnswer(int id, TestSubmissionAnswerRequest request, List<IFormFile> attachments)
        {
            if (request == null)
            {
                return new ApiResponse<TestSubmissionAnswerResponse>(
                    1, "Dữ liệu đầu vào không hợp lệ.", null, null);
            }

            try
            {
                var existingAnswer = _repository.GetTestSubmissionAnswerById(id);
                if (existingAnswer == null)
                {
                    return new ApiResponse<TestSubmissionAnswerResponse>(
                        1, "Không tìm thấy bản ghi cần cập nhật.", null, null);
                }

                // Cập nhật thông tin cơ bản
                _mapper.Map(request, existingAnswer);

                // Kiểm tra lại câu trả lời đúng nếu là trắc nghiệm
                bool isMultipleChoice = _testQuestionRepo.IsMultipleChoiceQuestion(request.QuestionId);
                bool isCorrect = false;
                string answerText = null;

                if (isMultipleChoice && request.SelectedAnswerId.HasValue)
                {
                    // Kiểm tra câu trả lời đúng
                    isCorrect = _testAnswerRepo.IsCorrectAnswer(request.SelectedAnswerId.Value);

                    // Lấy AnswerText từ TestAnswer
                    var selectedAnswer = _testAnswerRepo.GetAnswerById(request.SelectedAnswerId.Value);
                    answerText = selectedAnswer?.AnswerText;  // Lấy AnswerText nếu tìm thấy câu trả lời
                }

                existingAnswer.IsCorrect = isCorrect;
                existingAnswer.AnswerText = answerText;  // Cập nhật AnswerText vào bản ghi trả lời

                // Kiểm tra và xử lý file mới
                if (attachments != null && attachments.Any())
                {
                    // Xoá file cũ nếu có
                    var oldAttachments = _repository.GetAttachmentsBySubmissionAnswerId(id);
                    if (oldAttachments != null && oldAttachments.Any())
                    {
                        _repository.RemoveAttachments(oldAttachments.ToList());
                    }

                    // Thêm file mới (nếu có)
                    var newAttachments = new List<TestSubmissionAnswerAttachment>();

                    foreach (var file in attachments)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await file.CopyToAsync(memoryStream);
                            var base64 = Convert.ToBase64String(memoryStream.ToArray());

                            var extension = Path.GetExtension(file.FileName);
                            var newFileName = $"{DateTime.UtcNow:yyyyMMdd_HHmmssfff}_{Guid.NewGuid()}{extension}";

                            newAttachments.Add(new TestSubmissionAnswerAttachment
                            {
                                TestSubmissionAnswerId = id,
                                Filename = newFileName,
                                FileBase64 = base64,
                                CreatedAt = DateTime.UtcNow
                            });
                        }
                    }

                    _repository.AddAttachments(newAttachments);
                }
                else
                {
                    // Nếu không có file mới, giữ nguyên các file cũ
                    var existingAttachments = _repository.GetAttachmentsBySubmissionAnswerId(id);
                    if (existingAttachments != null && existingAttachments.Any())
                    {
                        foreach (var attachment in existingAttachments)
                        {
                            // Bạn có thể thêm logic xử lý hoặc thông báo nếu cần thiết
                            // Nhưng file cũ sẽ được giữ nguyên
                        }
                    }
                }

                // Lưu thay đổi vào database
                _repository.SaveChanges();

                var response = _mapper.Map<TestSubmissionAnswerResponse>(existingAnswer);
                return ApiResponse<TestSubmissionAnswerResponse>.Success(response);
            }
            catch (Exception ex)
            {
                return new ApiResponse<TestSubmissionAnswerResponse>(
                    1, "Lỗi hệ thống", null, new Dictionary<string, string[]>
                    {
                        { "Exception", new[] { "Không hợp lệ, vui lòng thử lại" } }
                    });
            }
        }

        public ApiResponse<ICollection<TestSubmissionAnswerResponse>> GetAnswersByTestId(long testId, int pageNumber, int pageSize)
        {
            var allAnswers = _repository.GetAnswersByTestId(testId);

            if (!allAnswers.Any())
            {
                return ApiResponse<ICollection<TestSubmissionAnswerResponse>>.NotFound("Không có câu trả lời nào cho test này.");
            }

            var totalItems = allAnswers.Count();
            var pagedAnswers = allAnswers
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var answerIds = pagedAnswers.Select(a => a.Id).ToList();
            var attachments = _repository.GetAttachmentsBySubmissionAnswerIds(answerIds);

            var response = pagedAnswers.Select(answer =>
            {
                var mapped = _mapper.Map<TestSubmissionAnswerResponse>(answer);
                mapped.Attachments = _mapper.Map<List<TestSubmissionAnswerAttachmentResponse>>(
                    attachments.Where(att => att.TestSubmissionAnswerId == answer.Id).ToList()
                );
                return mapped;
            }).ToList();

            return ApiResponse<ICollection<TestSubmissionAnswerResponse>>.Success(
                response,
                pageNumber,
                pageSize,
                totalItems
            );
        }

        public ApiResponse<TestSubmissionAnswerResponse> DeleteTestSubmissionAnswer(int id)
        {
            var result = _repository.DeleteTestSubmissionAnswer(id);
            return result
                ? ApiResponse<TestSubmissionAnswerResponse>.Success()
                : ApiResponse<TestSubmissionAnswerResponse>.NotFound("Không tìm thấy câu trả lời.");
        }
    }
}
