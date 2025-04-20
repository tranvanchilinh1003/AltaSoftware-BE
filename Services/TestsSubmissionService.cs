using AutoMapper;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;
using ISC_ELIB_SERVER.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ISC_ELIB_SERVER.Services
{
    public class TestsSubmissionService : ITestsSubmissionService
    {
        private readonly TestsSubmissionRepo _repository;
        private readonly TestAnswerRepo _testAnswerRepo;
        private readonly TestQuestionRepo _testQuestionRepo;
        private readonly CloudinaryService _cloudinaryService;
        private readonly TestSubmissionAnswerRepo _testSubmissionAnswerRepo;
        private readonly IMapper _mapper;

        public TestsSubmissionService(
            TestsSubmissionRepo repository, 
            IMapper mapper,
            TestAnswerRepo testAnswerRepo,
            TestQuestionRepo testQuestionRepo,
            TestSubmissionAnswerRepo testSubmissionAnswerRepo,
            CloudinaryService cloudinaryService)
        {
            _repository = repository;
            _testAnswerRepo = testAnswerRepo;
            _testQuestionRepo = testQuestionRepo;
            _testSubmissionAnswerRepo = testSubmissionAnswerRepo;
            _mapper = mapper;
            _cloudinaryService = cloudinaryService;
        }

        public ApiResponse<ICollection<TestsSubmissionResponse>> GetTestsSubmissiones(int page, int pageSize, string search, string sortColumn, string sortOrder)
        {
            var query = _repository.GetTestsSubmissions().AsQueryable();

            query = sortColumn switch
            {
                "Id" => sortOrder.ToLower() == "desc" ? query.OrderByDescending(ts => ts.Id) : query.OrderBy(ts => ts.Id),
                _ => query.OrderBy(us => us.Id)
            };

            var result = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var response = _mapper.Map<ICollection<TestsSubmissionResponse>>(result);

            return result.Any()
                    ? ApiResponse<ICollection<TestsSubmissionResponse>>.Success(response)
                    : ApiResponse<ICollection<TestsSubmissionResponse>>.NotFound("Không có dữ liệu");
        }

        public ApiResponse<TestsSubmissionResponse> GetTestsSubmissionById(long id)
        {
            var TestsSubmission = _repository.GetTestsSubmissionById(id);
            return TestsSubmission != null
                ? ApiResponse<TestsSubmissionResponse>.Success(_mapper.Map<TestsSubmissionResponse>(TestsSubmission))
                : ApiResponse<TestsSubmissionResponse>.NotFound($"Không tìm thấy thông tin #{id}");
        }

        public async Task<ApiResponse<List<TestsSubmissionResponse>>> GetByTestIdAsync(int testId)
        {
            var submissions = await _repository.GetByTestIdAsync(testId);
            if (submissions == null || !submissions.Any())
            {
                return ApiResponse<List<TestsSubmissionResponse>>.NotFound("Không tìm thấy bài nộp cho bài kiểm tra này.");
            }

            var responseData = _mapper.Map<List<TestsSubmissionResponse>>(submissions);
            return ApiResponse<List<TestsSubmissionResponse>>.Success(responseData);
        }

        //public ApiResponse<TestsSubmissionResponse> CreateTestsSubmission(TestsSubmissionRequest TestsSubmissionRequest)
        //{

        //    var TestsSubmissionEntity = _mapper.Map<TestsSubmission>(TestsSubmissionRequest);

        //    var created = _repository.CreateTestsSubmission(TestsSubmissionEntity);

        //    return ApiResponse<TestsSubmissionResponse>.Success(_mapper.Map<TestsSubmissionResponse>(created));
        //}

        public async Task<ApiResponse<TestsSubmissionResponse>> CreateTestsSubmission(
            TestsSubmissionRequest request, 
            List<TestSubmissionAnswerRequest> answerRequests)
        {
            if (request == null || answerRequests == null || !answerRequests.Any())
            {
                return new ApiResponse<TestsSubmissionResponse>(
                    1, "Dữ liệu đầu vào không hợp lệ.", null, null);
            }

            try
            {
                var testSubmission = _mapper.Map<TestsSubmission>(request);

                // Đảm bảo các giá trị null được xử lý đúng
                testSubmission.SubmittedAt = request.SubmittedAt ?? DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);
                testSubmission.Graded = request.Graded ?? false;
                testSubmission.Active = request.Active ?? true;

                var createdSubmission = _repository.CreateTestsSubmission(testSubmission);

                // 2. Xử lý và tạo câu trả lời và tệp đính kèm
                var submissionAnswers = new List<TestSubmissionsAnswer>();
                int correctCount = 0;

                for (int i = 0; i < answerRequests.Count; i++)
                {
                    var answerRequest = answerRequests[i];

                    // Kiểm tra xem có phải câu hỏi trắc nghiệm không
                    bool isMultipleChoice = _testQuestionRepo.IsMultipleChoiceQuestion(answerRequest.QuestionId);
                    bool isCorrect = false;

                    if (isMultipleChoice && answerRequest.SelectedAnswerId.HasValue)
                    {
                        // Kiểm tra xem câu trả lời đã chọn có đúng không
                        isCorrect = _testAnswerRepo.IsCorrectAnswer(answerRequest.SelectedAnswerId.Value);

                        // Lấy AnswerText từ bảng TestAnswer nếu chưa được cung cấp
                        if (string.IsNullOrEmpty(answerRequest.AnswerText))
                        {
                            var selectedAnswer = _testAnswerRepo.GetAnswerById(answerRequest.SelectedAnswerId.Value);
                            answerRequest.AnswerText = selectedAnswer?.AnswerText;
                        }
                    }

                    // Tạo bản ghi câu trả lời sử dụng AutoMapper
                    var submissionAnswer = _mapper.Map<TestSubmissionsAnswer>(answerRequest);

                    // Thiết lập các giá trị bổ sung không có trong DTO
                    submissionAnswer.SubmissionId = createdSubmission.Id;
                    Console.WriteLine($"SubmissionId: {submissionAnswer.SubmissionId}");
                    submissionAnswer.IsCorrect = isCorrect;
                    submissionAnswer.Active = true;

                    // Nếu không có điểm thì tính dựa trên kết quả đúng/sai
                    if (!submissionAnswer.Score.HasValue)
                    {
                        if (isMultipleChoice)
                        {
                            submissionAnswer.Score = isCorrect ? 10 : 0;
                        }
                        else
                        {
                            submissionAnswer.Score = 0; // hoặc null nếu bạn muốn xử lý ở giáo viên
                        }
                    }

                    var createdAnswer = _testSubmissionAnswerRepo.CreateTestSubmissionAnswer(submissionAnswer);
                    _repository.SaveChanges();
                    submissionAnswers.Add(createdAnswer);

                    if (isCorrect)
                    {
                        correctCount++;
                    }

                    // Xử lý tệp đính kèm cho câu trả lời này nếu có
                    if (answerRequest.Attachments != null && answerRequest.Attachments.Any())
                    {
                        var fileAttachments = new List<TestSubmissionAnswerAttachment>();

                        foreach (var file in answerRequest.Attachments)
                        {
                            using (var memoryStream = new MemoryStream())
                            {
                                file.CopyTo(memoryStream);
                                var base64 = Convert.ToBase64String(memoryStream.ToArray());
                                var newFileName = $"{DateTime.UtcNow:yyyyMMddHHmmssfff}_{file.FileName}";
                                var cloudinaryUrl = await _cloudinaryService.UploadBase64Async(base64, newFileName);

                                var attachment = new TestSubmissionAnswerAttachment
                                {
                                    TestSubmissionAnswerId = createdAnswer.Id,
                                    Filename = newFileName,
                                    FileBase64 = cloudinaryUrl,
                                    CreatedAt = DateTime.UtcNow
                                };

                                fileAttachments.Add(attachment);
                            }
                        }

                        _testSubmissionAnswerRepo.AddAttachments(fileAttachments);
                    }

                }

                // 3. Cập nhật thống kê bài nộp nếu chưa được cung cấp
                if (!request.CorrectAnswers.HasValue || !request.WrongAnswers.HasValue || !request.Score.HasValue)
                {
                    createdSubmission.CorrectAnswers = correctCount;
                    createdSubmission.WrongAnswers = answerRequests.Count - correctCount;

                    var totalQuestions = request.TotalQuestion ?? answerRequests.Count;
                    createdSubmission.TotalQuestion = totalQuestions;

                    if (!request.Score.HasValue)
                    {
                        createdSubmission.Score = TinhDiemTongThe(submissionAnswers, totalQuestions);
                    }

                    _repository.UpdateTestsSubmission(createdSubmission);
                }

                // Hoàn tất giao dịch
                _repository.SaveChanges();

                // Tải lại bài nộp kèm theo tất cả các mối quan hệ để xây dựng response đầy đủ
                var completeSubmission = _repository.GetTestSubmissionWithDetails(createdSubmission.Id);

                // Sử dụng AutoMapper để ánh xạ sang response
                var response = _mapper.Map<TestsSubmissionResponse>(completeSubmission);

                return ApiResponse<TestsSubmissionResponse>.Success(response);
            }
            catch (DbUpdateException ex)
            {
                return new ApiResponse<TestsSubmissionResponse>(
                    1,
                    "Lỗi khi lưu vào CSDL",
                    null,
                    new Dictionary<string, string[]>
                    {
                    { "Exception", new[] { ex.Message } },
                    { "InnerException", new[] { ex.InnerException?.Message ?? "Không có thông tin chi tiết" } }
                });
            }
        }

        private double TinhDiemTongThe(List<TestSubmissionsAnswer> answers, int? totalQuestions)
        {
            if (totalQuestions == null || totalQuestions == 0) return 0;

            double tongDiem = answers.Sum(a => a.Score ?? 0);
            double diemToiDa = totalQuestions.Value * 10; // Giả sử mỗi câu hỏi trị giá 10 điểm

            return Math.Round((tongDiem / diemToiDa) * 10, 2); // Trả về điểm thang 10, làm tròn 2 chữ số thập phân
        }

        public async Task<ApiResponse<TestsSubmissionResponse>> UpdateTestsSubmission(
            int submissionId,
            TestsSubmissionRequest request,
            List<TestSubmissionAnswerRequest>? answerRequests)
        {
            var existingSubmission = _repository.GetTestSubmissionWithDetails(submissionId);
            if (existingSubmission == null)
            {
                return new ApiResponse<TestsSubmissionResponse>(
                    1, "Bài nộp không tồn tại.", null, null);
            }

            try
            {
                // Cập nhật metadata
                existingSubmission.SubmittedAt = request.SubmittedAt ?? existingSubmission.SubmittedAt;
                existingSubmission.Graded = request.Graded ?? existingSubmission.Graded;
                existingSubmission.Active = request.Active ?? existingSubmission.Active;
                existingSubmission.TotalQuestion = request.TotalQuestion ?? existingSubmission.TotalQuestion;
                existingSubmission.StudentId = request.StudentId ?? existingSubmission.StudentId;
                existingSubmission.UserId = request.UserId ?? existingSubmission.UserId;

                int correctCount = 0;
                var updatedAnswers = new List<TestSubmissionsAnswer>();

                if (answerRequests != null && answerRequests.Any())
                {
                    // Truy xuất tất cả câu trả lời hiện có cho submission
                    var existingAnswers = _testSubmissionAnswerRepo.GetAnswersBySubmissionId(submissionId)
                        .ToDictionary(a => a.QuestionId);

                    foreach (var answerRequest in answerRequests)
                    {
                        bool isMultipleChoice = _testQuestionRepo.IsMultipleChoiceQuestion(answerRequest.QuestionId);
                        bool isCorrect = false;

                        if (isMultipleChoice && answerRequest.SelectedAnswerId.HasValue)
                        {
                            isCorrect = _testAnswerRepo.IsCorrectAnswer(answerRequest.SelectedAnswerId.Value);

                            if (string.IsNullOrEmpty(answerRequest.AnswerText))
                            {
                                var selectedAnswer = _testAnswerRepo.GetAnswerById(answerRequest.SelectedAnswerId.Value);
                                answerRequest.AnswerText = selectedAnswer?.AnswerText;
                            }
                        }

                        if (existingAnswers.TryGetValue(answerRequest.QuestionId, out var existingAnswer))
                        {
                            // So sánh và cập nhật nếu có thay đổi
                            bool hasChanges =
                                existingAnswer.SelectedAnswerId != answerRequest.SelectedAnswerId ||
                                existingAnswer.AnswerText != answerRequest.AnswerText ||
                                existingAnswer.TeacherComment != answerRequest.TeacherComment;

                            if (hasChanges)
                            {
                                existingAnswer.SelectedAnswerId = answerRequest.SelectedAnswerId;
                                existingAnswer.AnswerText = answerRequest.AnswerText;
                                existingAnswer.IsCorrect = isCorrect;
                                if (answerRequest.Score.HasValue)
                                {
                                    existingAnswer.Score = answerRequest.Score;
                                }
                                else if (isMultipleChoice)
                                {
                                    existingAnswer.Score = isCorrect ? 10 : 0;
                                }
                                existingAnswer.Active = true;
                                existingAnswer.TeacherComment = answerRequest.TeacherComment;

                                _testSubmissionAnswerRepo.UpdateTestSubmissionAnswer(existingAnswer);
                            }

                            updatedAnswers.Add(existingAnswer);
                        }
                        else
                        {
                            // Nếu không tồn tại -> tạo mới
                            var newAnswer = _mapper.Map<TestSubmissionsAnswer>(answerRequest);
                            newAnswer.SubmissionId = submissionId;
                            newAnswer.IsCorrect = isCorrect;
                            newAnswer.Active = true;
                            newAnswer.Score = newAnswer.Score ?? (isCorrect ? 10 : 0);

                            var createdAnswer = _testSubmissionAnswerRepo.CreateTestSubmissionAnswer(newAnswer);
                            updatedAnswers.Add(createdAnswer);

                            // Gán để xử lý file đính kèm
                            existingAnswer = createdAnswer;
                        }

                        if (isCorrect)
                            correctCount++;

                        // Xử lý file đính kèm mới
                        if (answerRequest.Attachments != null && answerRequest.Attachments.Any())
                        {
                            var fileAttachments = new List<TestSubmissionAnswerAttachment>();
                            foreach (var file in answerRequest.Attachments)
                            {
                                using (var memoryStream = new MemoryStream())
                                {
                                    file.CopyTo(memoryStream);
                                    var base64 = Convert.ToBase64String(memoryStream.ToArray());
                                    var newFileName = $"{DateTime.UtcNow:yyyyMMddHHmmssfff}_{file.FileName}";
                                    var cloudinaryUrl = await _cloudinaryService.UploadBase64Async(base64, newFileName);

                                    fileAttachments.Add(new TestSubmissionAnswerAttachment
                                    {
                                        TestSubmissionAnswerId = existingAnswer.Id,
                                        Filename = newFileName,
                                        FileBase64 = cloudinaryUrl,
                                        CreatedAt = DateTime.UtcNow
                                    });
                                }
                            }
                            _testSubmissionAnswerRepo.AddAttachments(fileAttachments);
                        }
                    }

                    existingSubmission.CorrectAnswers = correctCount;
                    existingSubmission.WrongAnswers = answerRequests.Count - correctCount;
                    existingSubmission.TotalQuestion = answerRequests.Count;

                    existingSubmission.Score = request.Score ?? TinhDiemTongThe(updatedAnswers, answerRequests.Count);
                }
                else if (request.Score.HasValue)
                {
                    existingSubmission.Score = request.Score;
                }

                _repository.UpdateTestsSubmission(existingSubmission);
                _repository.SaveChanges();

                var completeSubmission = _repository.GetTestSubmissionWithDetails(existingSubmission.Id);
                var response = _mapper.Map<TestsSubmissionResponse>(completeSubmission);

                return ApiResponse<TestsSubmissionResponse>.Success(response);
            }
            catch (DbUpdateException ex)
            {
                return new ApiResponse<TestsSubmissionResponse>(
                    1,
                    "Lỗi khi cập nhật dữ liệu.",
                    null,
                    new Dictionary<string, string[]>
                    {
                { "Exception", new[] { ex.Message } },
                { "InnerException", new[] { ex.InnerException?.Message ?? "Không có chi tiết lỗi." } }
                    });
            }
        }

        public ApiResponse<TestsSubmission> DeleteTestsSubmission(long id)
        {
            var success = _repository.DeleteTestsSubmission(id);
            return success
                ? ApiResponse<TestsSubmission>.Success()
                : ApiResponse<TestsSubmission>.NotFound("Không tìm thấy");
        }
    }
}
