using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using ISC_ELIB_SERVER.Enums;
using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;
using OfficeOpenXml;
using System.Collections.Generic;
using System.Linq;

namespace ISC_ELIB_SERVER.Services
{
    public class TestAnswerService
    {
        private readonly TestAnswerRepo _repository;
        private readonly isc_dbContext _context;


        public TestAnswerService(TestAnswerRepo repository, isc_dbContext context)
        {
            _repository = repository;
             _context = context;
        }

        //  Lấy danh sách câu trả lời theo QuestionId
        public ApiResponse<List<TestAnswerResponse>> GetAnswersByQuestion(int questionId)
        {
            var answers = _repository.GetAnswersByQuestion(questionId);

            var response = answers.Select(a => new TestAnswerResponse
            {
                Id = a.Id,
                QuestionId = a.QuestionId ?? 0,
                AnswerText = a.AnswerText ?? "",
                IsCorrect = a.IsCorrect ?? false
             
            }).ToList();

            return response.Any()
                ? ApiResponse<List<TestAnswerResponse>>.Success(response)
                : ApiResponse<List<TestAnswerResponse>>.NotFound("Không có câu trả lời nào cho câu hỏi này.");
        }

        //  Tạo câu trả lời
      public ApiResponse<TestAnswer> CreateTestAnswer(TestAnswerRequest request)
        {
            var newAnswer = new TestAnswer
            {
                QuestionId = request.QuestionId,
                AnswerText = request.AnswerText,
                IsCorrect = request.IsCorrect
            };

            var createdAnswer = _repository.CreateTestAnswer(newAnswer);
            return ApiResponse<TestAnswer>.Success(createdAnswer);
        }

        //  Cập nhật câu trả lời
        public ApiResponse<TestAnswerResponse> UpdateTestAnswer(int id, TestAnswerRequest request)
        {
            var existingAnswer = _repository.GetAnswerById(id);
            if (existingAnswer == null)
                return ApiResponse<TestAnswerResponse>.NotFound("Không tìm thấy câu trả lời.");

            // Cập nhật thông tin câu trả lời
            existingAnswer.AnswerText = request.AnswerText;
            existingAnswer.IsCorrect = request.IsCorrect;

            var updatedAnswer = _repository.UpdateAnswer(existingAnswer);

            var response = new TestAnswerResponse
            {
                Id = updatedAnswer.Id,
                QuestionId = updatedAnswer.QuestionId ?? 0,
                AnswerText = updatedAnswer.AnswerText ?? "",
                IsCorrect = updatedAnswer.IsCorrect ?? false
            };

            return ApiResponse<TestAnswerResponse>.Success(response);
        }

        //  Xóa câu trả lời
        public ApiResponse<bool> DeleteTestAnswer(int id)
        {
            var existingAnswer = _repository.GetAnswerById(id);
            if (existingAnswer == null)
                return ApiResponse<bool>.NotFound("Không tìm thấy câu trả lời.");

            var deleted = _repository.DeleteAnswer(id);
            return deleted
                ? ApiResponse<bool>.Success(true)
                : ApiResponse<bool>.Error(new Dictionary<string, string[]> { { "message", new[] { "Xóa câu trả lời thất bại." } } });
        }

           public async Task<ApiResponse<string>> ImportFromExcelAsync(IFormFile file, int testId, string questionType)
            {
                if (file == null || file.Length == 0)
                    return ApiResponse<string>.BadRequest("File không hợp lệ");

                if (!Enum.TryParse<QuestionType>(questionType, out var parsedType))
                    return ApiResponse<string>.BadRequest("Giá trị QuestionType không hợp lệ");

                //  XÓA toàn bộ câu hỏi và đáp án của testId
                var questionsToRemove = _context.TestQuestions.Where(q => q.TestId == testId).ToList();
                var questionIds = questionsToRemove.Select(q => q.Id).ToList();

                var answersToRemove = _context.TestAnswers.Where(a => questionIds.Contains(a.QuestionId ?? 0)).ToList();

                _context.TestAnswers.RemoveRange(answersToRemove);
                _context.TestQuestions.RemoveRange(questionsToRemove);
                await _context.SaveChangesAsync();

                //  TẠO THƯ MỤC nếu chưa có
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", "TestFiles");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                //  Lưu file vào thư mục
               var originalFileName = Path.GetFileNameWithoutExtension(file.FileName);
                var extension = Path.GetExtension(file.FileName);
                var safeFileName = $"{originalFileName}{extension}";
                var filePath = Path.Combine(uploadsFolder, safeFileName);
                int counter = 1;

                // Nếu file đã tồn tại, thêm số đếm vào tên file
                while (System.IO.File.Exists(filePath))
                {
                    safeFileName = $"{originalFileName}-{counter}{extension}";
                    filePath = Path.Combine(uploadsFolder, safeFileName);
                    counter++;
                }
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                //  Lưu vào bảng test_file
                var testFile = new TestFile
                {
                    TestId = testId,
                    FileUrl = $"/Uploads/TestFiles/{safeFileName}",
                    Active = true
                };
                _context.TestFiles.Add(testFile);
                await _context.SaveChangesAsync();

                //  Đọc nội dung Excel
                using var stream = new MemoryStream();
                await file.CopyToAsync(stream);
                using var package = new ExcelPackage(stream);
                var worksheet = package.Workbook.Worksheets[0];
                var rowCount = worksheet.Dimension.Rows;

                for (int row = 2; row <= rowCount; row++)
                {
                    var questionText = worksheet.Cells[row, 2].Text.Trim();
                    var questionTypeText = worksheet.Cells[row, 3].Text.Trim();

                    if (string.IsNullOrEmpty(questionText) || string.IsNullOrEmpty(questionTypeText))
                        continue;

                    if (!Enum.TryParse<QuestionType>(questionTypeText, out var innerParsedType))
                        continue;

                    var newQuestion = new TestQuestion
                    {
                        TestId = testId,
                        QuestionText = questionText,
                        QuestionType = innerParsedType,
                        Active = true
                    };

                    _context.TestQuestions.Add(newQuestion);
                    await _context.SaveChangesAsync();

                    if (innerParsedType == QuestionType.TracNghiem)
                    {
                        var answers = new List<(string Label, string Text)>
                        {
                            ("A", worksheet.Cells[row, 4].Text.Trim()),
                            ("B", worksheet.Cells[row, 5].Text.Trim()),
                            ("C", worksheet.Cells[row, 6].Text.Trim()),
                            ("D", worksheet.Cells[row, 7].Text.Trim())
                        };

                        var correctAnswerLabel = worksheet.Cells[row, 8].Text.Trim().ToUpper();

                        foreach (var answer in answers)
                        {
                            if (string.IsNullOrEmpty(answer.Text)) continue;

                            var isCorrect = answer.Label == correctAnswerLabel;

                            _context.TestAnswers.Add(new TestAnswer
                            {
                                QuestionId = newQuestion.Id,
                                AnswerText = answer.Text,
                                IsCorrect = isCorrect
                            });
                        }
                    }

                    await _context.SaveChangesAsync();
                }

                return ApiResponse<string>.Success("Import thành công");
            }




    }
}
