using ISC_ELIB_SERVER.Models;
using ISC_ELIB_SERVER.Repositories;
using ISC_ELIB_SERVER.DTOs.Requests;
using ISC_ELIB_SERVER.DTOs.Responses;
using AutoMapper;

namespace ISC_ELIB_SERVER.Services
{
    public class QuestionQaService : IQuestionQaService
    {
        private readonly QuestionQaRepo _repository;
        private readonly IMapper _mapper;
        private readonly isc_dbContext _context;
        private readonly UserRepo _userRepo;
        private readonly QuestionViewRepo _viewRepo;
        private readonly long _maxImageSize = 2 * 1024 * 1024; // 2MB
        private readonly string[] _allowedImageTypes = new[] { ".jpg", ".jpeg", ".png", ".webp" };

        public QuestionQaService(QuestionQaRepo repository,  UserRepo userRepo, QuestionViewRepo viewRepo, IMapper mapper, isc_dbContext context)
            {
                _repository = repository;
                _userRepo = userRepo;
                _viewRepo = viewRepo;
                _context = context;
               
            }

      public ApiResponse<ICollection<QuestionQaResponse>> GetQuestions(
            int userId, int page, int pageSize, string search, string sortColumn, string sortOrder, int? classId, int? subjectId)
        {
            var query = _repository.GetQuestions(classId, subjectId).AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(q => q.Content.ToLower().Contains(search.ToLower()));
            }

            var questionsWithViews = query
                .Select(q => new
                {
                    Question = q,
                    IsRead = _viewRepo.HasUserViewed(q.Id, userId),
                    HasAnswer = q.AnswersQas.Any(),
                    ImageUrls = q.QuestionImagesQas.Select(i => i.ImageUrl).ToList() //  Lấy danh sách ảnh
                })
                .OrderByDescending(q => q.Question.CreateAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var result = questionsWithViews.Select(q => new QuestionQaResponse
            {
                Id = q.Question.Id,
                Content = q.Question.Content,
                CreateAt = q.Question.CreateAt ?? DateTime.UtcNow,
                UserId = q.Question.UserId ?? 0,
                UserName = q.Question.User?.FullName ?? "Unknown",
                UserAvatar = q.Question.User?.AvatarUrl ?? "https://via.placeholder.com/50",
                ViewCount = _viewRepo.GetViewCount(q.Question.Id),
                IsRead = q.IsRead,
                HasAnswer = q.HasAnswer,
                ImageUrls = q.ImageUrls //  Fix lỗi `null`
            }).ToList();

            return result.Any()
                ? ApiResponse<ICollection<QuestionQaResponse>>.Success(result)
                : ApiResponse<ICollection<QuestionQaResponse>>.NotFound("Không có dữ liệu");
        }


         public ApiResponse<ICollection<QuestionQaResponse>> GetAnsweredQuestions(
            int userId, int page, int pageSize, int? classId, int? subjectId)
        {
            var query = _repository.GetAnsweredQuestions(classId, subjectId).AsQueryable();

            var questionsWithViews = query
                .Select(q => new
                {
                    Question = q,
                    IsRead = _viewRepo.HasUserViewed(q.Id, userId)
                })
                .OrderByDescending(q => q.Question.CreateAt) // 🏆 Câu hỏi mới nhất lên trước
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var result = questionsWithViews.Select(q => new QuestionQaResponse
            {
                Id = q.Question.Id,
                Content = q.Question.Content,
                CreateAt = q.Question.CreateAt,
                UserId = q.Question.UserId,
                UserName = q.Question.User?.FullName ?? "Unknown",
                UserAvatar = q.Question.User?.AvatarUrl,
                ViewCount = _viewRepo.GetViewCount(q.Question.Id),
                IsRead = q.IsRead,
                HasAnswer = true // Chắc chắn đã có câu trả lời
            }).ToList();

            return result.Any()
                ? ApiResponse<ICollection<QuestionQaResponse>>.Success(result)
                : ApiResponse<ICollection<QuestionQaResponse>>.NotFound("Không có câu hỏi nào đã trả lời.");
        }


      public ApiResponse<QuestionQaResponse> GetQuestionById(long id)
        {
            var question = _repository.GetQuestionById(id);
            if (question == null)
            {
                return ApiResponse<QuestionQaResponse>.NotFound($"Không tìm thấy câu hỏi #{id}");
            }

            var response = new QuestionQaResponse
            {
                Id = question.Id,
                Content = question.Content,
                CreateAt = question.CreateAt,
                UserId = question.UserId,
                UserName = question.User != null ? question.User.FullName : "Unknown",
                UserAvatar = question.User != null ? question.User.AvatarUrl : null,
                ViewCount = _viewRepo.GetViewCount(question.Id),
                HasAnswer = question.AnswersQas.Any(),
                ImageUrls = question.QuestionImagesQas.Select(img => img.ImageUrl).ToList() //Thêm danh sách ảnh
            };

            return ApiResponse<QuestionQaResponse>.Success(response);
        }

         public async Task<ApiResponse<ApiResponse<string>>> CreateQuestion(QuestionQaRequest questionRequest, List<IFormFile> files, int userId)
        {
            if (string.IsNullOrWhiteSpace(questionRequest.Content))
                return ApiResponse<ApiResponse<string>>.BadRequest("Nội dung câu hỏi không được để trống.");

            var isValidClassSubject = _context.TeachingAssignments
                .Any(t => t.ClassId == questionRequest.ClassId && t.SubjectId == questionRequest.SubjectId && t.Active);

            if (!isValidClassSubject)
                return ApiResponse<ApiResponse<string>>.BadRequest("Lớp học này không có môn học này.");


                // Chuyển file ảnh thành Base64
                List<string> imageBase64s = new List<string>();
                if (files != null && files.Count > 0)
                {
                    foreach (var file in files)
                    {
                         var extension = Path.GetExtension(file.FileName).ToLower();

                        // Kiểm tra định dạng ảnh
                        if (!_allowedImageTypes.Contains(extension))
                        {
                            return ApiResponse<ApiResponse<string>>.BadRequest($"Chỉ cho phép định dạng ảnh: JPG, JPEG, PNG, WEBP");
                        }

                        // Kiểm tra dung lượng
                        if (file.Length > _maxImageSize)
                        {
                            return ApiResponse<ApiResponse<string>>.BadRequest("Ảnh vượt quá kích thước tối đa 2MB");
                        }
                        using (var ms = new MemoryStream())
                        {
                            await file.CopyToAsync(ms);
                            string base64String = Convert.ToBase64String(ms.ToArray());
                            imageBase64s.Add($"data:{file.ContentType};base64,{base64String}");
                        }
                    }
                }

                // Chuyển đổi DateTime để tránh lỗi PostgreSQL
                var createAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);

                var newQuestion = new QuestionQa
                {
                    Content = questionRequest.Content,
                    UserId = userId, //  Gán userId từ token
                    SubjectId = questionRequest.SubjectId,
                    CreateAt = createAt,
                    Active = true
                };

                var createdQuestion = _repository.CreateQuestion(newQuestion, imageBase64s);

                var response = new QuestionQaResponse
                {
                    Id = createdQuestion.Id,
                    Content = createdQuestion.Content,
                    CreateAt = createdQuestion.CreateAt ?? DateTime.UtcNow,
                    UserId = createdQuestion.UserId ?? 0,
                    UserAvatar = createdQuestion.User?.AvatarUrl ?? "https://via.placeholder.com/50",
                    UserName = createdQuestion.User?.FullName ?? "Unknown",
                };

                return ApiResponse<ApiResponse<string>>.Success();
            }

        public ApiResponse<QuestionQaResponse> UpdateQuestion(long id, QuestionQaRequest request)
        {
            var existing = _repository.GetQuestionById(id);
            if (existing == null)
            {
                return ApiResponse<QuestionQaResponse>.NotFound("Câu hỏi không tồn tại");
            }

            existing.Content = request.Content;
            existing.SubjectId = request.SubjectId;
           
            existing.CreateAt = DateTime.Now;

            var updated = _repository.UpdateQuestion(existing);
            return ApiResponse<QuestionQaResponse>.Success(_mapper.Map<QuestionQaResponse>(updated));
        }

     public ApiResponse<QuestionQaResponse> DeleteQuestion(long id)
        {
            var success = _repository.DeleteQuestion(id);
            return success
                ? ApiResponse<QuestionQaResponse>.Success()
                : ApiResponse<QuestionQaResponse>.NotFound("Không tìm thấy câu hỏi để xóa");
        }

        public ApiResponse<QuestionQaResponse> GetQuestionByIdForUser(int id, int userId)
            {
                var question = _repository.GetQuestionById(id);
                if (question == null)
                    return ApiResponse<QuestionQaResponse>.NotFound($"Không tìm thấy câu hỏi #{id}");

                // Thêm lượt xem nếu chưa được xem bởi người dùng này
                _viewRepo.AddView(id, userId);

                var response = new QuestionQaResponse
                {
                    Id = question.Id,
                    Content = question.Content,
                    CreateAt = question.CreateAt,
                    UserId = question.UserId,
                    UserName = question.User != null ? question.User.FullName : "Unknown",
                    UserAvatar = question.User != null ? question.User.AvatarUrl : null,
                    ViewCount = _viewRepo.GetViewCount(question.Id)
                };

                return ApiResponse<QuestionQaResponse>.Success(response);
            }
     public ApiResponse<ICollection<QuestionQaResponse>> SearchQuestionsByUserName(
            int userId, string userName, bool onlyAnswered, int page, int pageSize, int? classId, int? subjectId)
        {
            var query = _repository.SearchQuestionsByUserName(userName, onlyAnswered, classId, subjectId).AsQueryable();

            var questionsWithViews = query
                .Select(q => new
                {
                    Question = q,
                    IsRead = _viewRepo.HasUserViewed(q.Id, userId)
                })
                .OrderByDescending(q => q.Question.CreateAt) // 🏆 Mới nhất lên trước
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var result = questionsWithViews.Select(q => new QuestionQaResponse
            {
                Id = q.Question.Id,
                Content = q.Question.Content,
                CreateAt = q.Question.CreateAt,
                UserId = q.Question.UserId,
                UserName = q.Question.User?.FullName ?? "Unknown",
                UserAvatar = q.Question.User?.AvatarUrl,
                ViewCount = _viewRepo.GetViewCount(q.Question.Id),
                IsRead = q.IsRead,
                HasAnswer = q.Question.AnswersQas.Any()
            }).ToList();

            return result.Any()
                ? ApiResponse<ICollection<QuestionQaResponse>>.Success(result)
                : ApiResponse<ICollection<QuestionQaResponse>>.NotFound("Không tìm thấy câu hỏi nào.");
        }

       public ApiResponse<ICollection<QuestionQaResponse>> GetRecentQuestions(
            int userId, int page, int pageSize, int? classId, int? subjectId)
        {
            var query = _repository.GetRecentQuestions(classId, subjectId).AsQueryable();

            var questionsWithViews = query
                .Select(q => new
                {
                    Question = q,
                    IsRead = _viewRepo.HasUserViewed(q.Id, userId)
                })
                .OrderByDescending(q => q.Question.CreateAt) // 🏆 Câu hỏi mới nhất lên trước
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var result = questionsWithViews.Select(q => new QuestionQaResponse
            {
                Id = q.Question.Id,
                Content = q.Question.Content,
                CreateAt = q.Question.CreateAt,
                UserId = q.Question.UserId,
                UserName = q.Question.User?.FullName ?? "Unknown",
                UserAvatar = q.Question.User?.AvatarUrl,
                ViewCount = _viewRepo.GetViewCount(q.Question.Id),
                IsRead = q.IsRead,
                HasAnswer = q.Question.AnswersQas.Any()
            }).ToList();

            return result.Any()
                ? ApiResponse<ICollection<QuestionQaResponse>>.Success(result)
                : ApiResponse<ICollection<QuestionQaResponse>>.NotFound("Không có câu hỏi nào gần đây.");
        }


    }
}
